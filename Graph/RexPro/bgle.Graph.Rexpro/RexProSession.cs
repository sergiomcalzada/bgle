using System;

namespace bgle.Graph.Rexpro
{
    public class RexProSession : IDisposable
    {
        private bool disposed;

        public RexProSession(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }

        public bool Killed { get; set; }

        public EventHandler KillSession;

        private void OnKillSession()
        {
            var handler = this.KillSession;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.OnKillSession();
                }
            }
            this.disposed = true;
        }

        #endregion
    }
}