using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace CsvExcelTransformer.Classes
{
    /// <summary>
    /// Implements the INotifyPropertyChanged interface and exposes a
    /// RaisePropertyChanged method for derived classes to raise the
    /// PropertyChange event. The event arguments created by this class are
    /// cached to prevent managed heap fragmentation.
    /// </summary>
    public abstract class NotifyObject : INotifyPropertyChanged
    {
        #region Public Members

        /// <summary>
        /// Raised when a public property of this object is set.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Returns an instance of PropertyChangedEventArgs for the specified
        /// property name.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to create event args for.
        /// </param>
        public static PropertyChangedEventArgs
            GetPropertyChangedEventArgs(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException(
                    "propertyName cannot be null or empty.");

            PropertyChangedEventArgs args;

            // Get the event args from the cache, creating them and adding to
            // the cache if necessary.
            lock (m_LockObject)
            {
                bool isCached = m_EventArgCache.ContainsKey(propertyName);
                if (isCached == false)
                {
                    m_EventArgCache.Add(
                        propertyName,
                        new PropertyChangedEventArgs(propertyName));
                }

                args = m_EventArgCache[propertyName];
            }

            return args;
        }

        #endregion Public Members

        #region Private Members

        private const string ERROR_MSG = "{0} is not a public property of {1}";
        private static readonly Dictionary<string, PropertyChangedEventArgs> m_EventArgCache;
        private static readonly object m_LockObject = new object();

        #endregion Private Members

        #region constructors

        protected NotifyObject()
        {
        }

        static NotifyObject()
        {
            m_EventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        #endregion constructors

        #region Protected Methods

        /// <summary>
        /// Derived classes can override this method to execute logic after a
        /// property is set. The base implementation does nothing.
        /// </summary>
        /// <param name="propertyName">
        /// The property which was changed.
        /// </param>
        protected virtual void AfterPropertyChanged(string propertyName)
        {
        }

        /// <summary>
        /// Attempts to raise the PropertyChanged event, and invokes the virtual
        /// AfterPropertyChanged method, regardless of whether the event was
        /// raised or not.
        /// </summary>
        /// <param name="propertyName">
        /// The property which was changed.
        /// </param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.VerifyProperty(propertyName);

            PropertyChangedEventHandler? handler = this.PropertyChanged;

            if (handler != null)
            {
                // Get the cached event args.
                PropertyChangedEventArgs args =
                    GetPropertyChangedEventArgs(propertyName);

                // Raise the PropertyChanged event.
                handler(this, args);
            }

            this.AfterPropertyChanged(propertyName);
        }

        #endregion Protected Methods

        #region Private Methods

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            Type type = this.GetType();

            // Look for a public property with the specified name.

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

            PropertyInfo propInfo = type.GetProperty(propertyName);

#pragma warning restore CS8600

            if (propInfo is null)
            {
                // The property could not be found, so alert the developer of
                // the problem.

                string msg = string.Format(
                    ERROR_MSG,
                    propertyName,
                    type.FullName);

                Debug.Fail(msg);
            }
        }

        #endregion Private Methods
    }
}
