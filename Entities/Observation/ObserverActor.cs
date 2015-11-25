using System;
using Akka.Actor;

namespace Entities.Observation
{
    /// <summary>
    /// Observes a subject actor sending messages of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObserverActor<T> : ReceiveActor
    {
        private Subscribe _subscription;

        public ObserverActor()
        {
            Become(Unsubscribed);
        }

        private void Unsubscribed()
        {
            Receive<Subscribe>(msg =>
            {
                Context.LogMessageDebug(msg);
                _subscription = msg;
                _subscription.Subject.Tell(new Observe());
                Become(Subscribed);
            });
        }

        private void Subscribed()
        {
            Receive<UnSubscribe>(msg =>
            {
                Context.LogMessageDebug(msg);
                _subscription.Subject.Tell(new UnObserve());
                _subscription = null;
                Become(Unsubscribed);
            });

            Receive<T>(msg =>
            {
                _subscription.Callback(msg);
            });
        }

        #region Messages
        /// <summary>
        /// Unsubscribe from the observer
        /// </summary>
        public class UnSubscribe
        {
        }

        /// <summary>
        /// Subscribe for events from the subject and setup the callback to execute on receipt of the message T
        /// </summary>
        public class Subscribe
        {
            public Subscribe(Action<T> callback, IActorRef subject)
            {
                Callback = callback;
                Subject = subject;
            }

            /// <summary>
            /// The subject actor of the observer
            /// </summary>
            public IActorRef Subject { get; }

            /// <summary>
            /// The callback that will be executed upon observation of the given message
            /// </summary>
            public Action<T> Callback { get; }
        }

        #endregion
    }
}