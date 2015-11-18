using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Persistence;

namespace Entities
{
    public class WorldPrefixPersistanceActor : PersistentActor
    {
        private State _state = new State();
        private const string PersistenceIdName = "WorldPrefixPersistenceActor";

        protected override bool ReceiveRecover(object message)
        {
            var s = message as string;
            var snapShot = message as SnapshotOffer;
            var recoverComplete = message as RecoveryCompleted;
            if (s != null)
            {
                Context.LogMessageDebug(s);
                Context.Parent.Tell(new Recovering());
                UpdateState(s);
            }
            else if (snapShot != null)
            {
                var state = snapShot.Snapshot as State;
                if (state != null)
                {
                    Context.LogMessageDebug(state, "Recieve recover");
                    _state = state;
                }
                else
                {
                    Context.LogMessageDebug(snapShot, "failed to parse");
                    return false;
                }
            }
            else if (recoverComplete != null)
            {
                Context.LogMessageDebug(recoverComplete);
                Context.Parent.Tell(new WorldPrefixPersistentRecoveryComplete(_state.Prefixes.ToArray()));
            }
            else
            {
                Context.LogMessageDebug(message, "failed to parse");
                return false;
            }
            
            return true;
        }

        public class Recovering
        {
        }

        protected override bool ReceiveCommand(object message)
        {
            var prefixMessage = message as PostNewPrefixMessage;
            var storeMessage = message as PostStoreStateMessage;
            var queryMessage = message as QueryPrefixes;
            
            if (prefixMessage != null)
            {
                Persist(prefixMessage.Prefix, UpdateState);
            }
            else if (storeMessage != null)
            {
                Context.LogMessageDebug(message);
                SaveSnapshot(_state);
            }
            else if (queryMessage != null)
            {
                Context.LogMessageDebug(message);
                Sender.Tell(new PostQueryResultsMessage(_state.Prefixes, queryMessage.Sender), Self);
            }
            else if (message is SaveSnapshotSuccess)
            {
                Context.LogMessageDebug(message);
                Context.Parent.Tell(new StateSavedMessage());
            }
            else if (message is SaveSnapshotFailure) { Context.LogMessageDebug(message); }
            else
            {
                return false;
            }

            return true;
        }

        private void UpdateState(string prefix)
        {
            _state.Update(prefix);
            Context.Parent.Tell(new PrefixPersisted(prefix));
        }

        public override string PersistenceId => PersistenceIdName;

        /// <summary>
        /// The state of a <see cref="WorldPrefixPersistanceActor"/>
        /// </summary>
        public class State
        {
           public List<string> Prefixes { get; set; } = new List<string>();

            /// <summary>
            /// Updates the state with the newest first.
            /// </summary>
            /// <param name="prefix"></param>
            public void Update(string prefix)
            {
                var newState = new List<string>() { prefix };
                newState.AddRange(Prefixes);
                Prefixes = newState;
            }
        }

        /// <summary>
        /// Posts a new prefix to the persistance actor
        /// </summary>
        public class PostNewPrefixMessage
        {
            public string Prefix { get; }

            public PostNewPrefixMessage(string prefix)
            {
                Prefix = prefix;
            }
        }

        public class WorldPrefixPersistentRecoveryComplete
        {
            public string[] Prefixes { get;  }


            public WorldPrefixPersistentRecoveryComplete(string[] prefixes)
            {
                Prefixes = prefixes;
            }
        }

        /// <summary>
        /// Posts the message telling the <see cref="WorldPrefixPersistanceActor"/> to store its state
        /// </summary>
        public class PostStoreStateMessage
        {
        }

        /// <summary>
        /// Queries the <see cref="WorldPrefixPersistanceActor"/> for the prefixes it contains
        /// </summary>
        public class QueryPrefixes
        {
            public IActorRef Sender { get;  }

            public QueryPrefixes(IActorRef sender)
            {
                Sender = sender;
            }
        }

        public class PostQueryResultsMessage
        {
            public List<string> Prefixes { get; }
            public IActorRef Sender { get; }

            public PostQueryResultsMessage(List<string> prefixes, IActorRef sender)
            {
                Prefixes = prefixes;
                Sender = sender;
            }
        }

        public class StateSavedMessage
        {
        }

        public class PrefixPersisted
        {
            public string Prefix { get; }

            public PrefixPersisted(string prefix)
            {
                Prefix = prefix;
            }
        }
    }
}