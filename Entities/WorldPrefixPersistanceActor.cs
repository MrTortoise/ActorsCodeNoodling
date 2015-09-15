using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka;
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
            if (s != null)
            {
                UpdateState(s);
            }
            else if (snapShot != null)
            {
                var state = snapShot.Snapshot as State;
                if (state != null)
                {
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
            var prefixMessage = message as PostNewPrefixMessage;
            var storeMessage = message as PostStoreStateMessage;
            var queryMessage = message as QueryPrefixes;
            if (prefixMessage != null)
            {
                Persist(prefixMessage.Prefix, UpdateState);
            }
            else if (storeMessage != null)
            {
                SaveSnapshot(_state); 
            }
            else if (queryMessage != null)
            {
                Sender.Tell(new PostQueryResultsMessage(_state.Prefixes), Self);
            }
            else
            {
                return false;
            }

            return true;
        }



        private void UpdateState(string val)
        {
            _state.Update(val);
        }

        public override string PersistenceId => PersistenceIdName;

        /// <summary>
        /// The state of a <see cref="WorldPrefixPersistanceActor"/>
        /// </summary>
        public class State
        {
            List<string> _prefixes = new List<string>();

            public List<string> Prefixes => _prefixes;

            /// <summary>
            /// Updates the state with the newest first.
            /// </summary>
            /// <param name="val"></param>
            public void Update(string val)
            {
                var newState = new List<string>() { val };
                newState.AddRange(_prefixes);
                _prefixes = newState;
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
    }
}