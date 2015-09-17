using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Persistence;



namespace Entities
{
    public class WorldPrefixPersistanceActor : PersistentActor
    {
        private State _state = new State();
        private const string PersistenceIdName = "WorldPrefixPersistenceActor";
        private readonly ILoggingAdapter log = Context.GetLogger(new SerilogLogMessageFormatter());

        protected override bool ReceiveRecover(object message)
        {
            var s = message as string;
            var snapShot = message as SnapshotOffer;
            if (s != null)
            {
                log.Debug("{Id}: Recover string {String}", Self.Path, s);
                UpdateState(s);
            }
            else if (snapShot != null)
            {
                var state = snapShot.Snapshot as State;
                if (state != null)
                {
                    log.Debug("{Id}: Recover snapshot {@Snapshot}",Self.Path, state);
                    _state = state;
                }
                else
                {
                    return false;
                }
            }
            else return false;

            return true;
        }

        protected override bool ReceiveCommand(object message)
        {
            log.Debug("{Id}: message recieved {@message}", Self.Path, message);
            var prefixMessage = message as PostNewPrefixMessage;
            var storeMessage = message as PostStoreStateMessage;
            var queryMessage = message as QueryPrefixes;
            if (prefixMessage != null)
            {
                log.Debug("{Id}: Receive prefix {Prefix}", Self.Path, prefixMessage.Prefix);
                Persist(prefixMessage.Prefix, UpdateState);
            }
            else if (storeMessage != null)
            {
                log.Debug("{Id}: Receive store snapshot", Self.Path);
                SaveSnapshot(_state);
                Sender.Tell(new StateSavedMessage(), Self);
            }
            else if (queryMessage != null)
            {
                log.Debug("{Id}: Receive query", Self.Path);
                Sender.Tell(new PostQueryResultsMessage(_state.Prefixes), Self);
            }
            else if (message is SaveSnapshotFailure || message is SaveSnapshotSuccess) { }
            else
            {
                return false;
            }

            return true;
        }



        private void UpdateState(string prefix)
        {
            log.Debug("update state: {prefix}",prefix);
            _state.Update(prefix);
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
        }

        public class PostQueryResultsMessage
        {
            public List<string> Prefixes { get; }

            public PostQueryResultsMessage(List<string> prefixes)
            {
                Prefixes = prefixes;
            }
        }

        public class StateSavedMessage
        {
        }
    }
}