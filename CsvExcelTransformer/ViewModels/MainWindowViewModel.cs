using System;
using System.Timers;
using CsvExcelTransformer.Classes;
using WpfPageTransitions;

namespace CsvExcelTransformer.ViewModels
{
    public class MainWindowViewModel : NotifyObject
    {
        #region Private Members
        
        private const string m_BaseTitle = " CSV Excel Transformer";
        private PageTransition m_PageTransition;

        private const int MAX_STATUS_TEXT_COUNT = 8;
        private int m_StatusTextCurrentCount = 0;
        private readonly Timer m_StatusTextTimer = new Timer();

        private bool m_ShowProgressBar = false;
        private string m_StatusText = string.Empty;
        private string m_TitleText = m_BaseTitle;

        #endregion Private Members

        #region Public Properties

        public bool ShowProgressBar
        {
            get => m_ShowProgressBar;

            set
            {
                if (m_ShowProgressBar != value)
                {
                    m_ShowProgressBar = value;

                    RaisePropertyChanged(nameof(ShowProgressBar));
                }
            }
        }

        public string StatusText
        {
            get => m_StatusText;

            set
            {
                if (string.IsNullOrWhiteSpace(m_StatusText) ||
                    m_StatusText.Equals(value, StringComparison.OrdinalIgnoreCase) == false)
                {
                    m_StatusText = value;

                    RaisePropertyChanged(nameof(StatusText));
                }
            }
        }

        public string TitleText 
        { 
            get => m_TitleText;

            internal set
            {
                if (string.IsNullOrWhiteSpace(m_TitleText) ||
                    m_TitleText != value)
                {
                    m_TitleText = value;

                    RaisePropertyChanged(nameof(TitleText));
                }
            }
        }

        #endregion Public Properties

        #region constructor

        internal MainWindowViewModel(PageTransition pageTransitionControl)
        {
            m_PageTransition = pageTransitionControl;

            m_StatusTextTimer.Interval = 1000.0d;
            m_StatusTextTimer.Elapsed += StatusTextTimer_Elapsed;
        }

        #endregion constructor

        #region Private Methods

        private void StatusTextTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if ((++m_StatusTextCurrentCount) >= MAX_STATUS_TEXT_COUNT)
            {
                m_StatusTextTimer.Stop();

                StatusText = string.Empty;
            }
        }

        #endregion Private Methods

        #region Public Methods

        internal void SetStatusText(string text, bool autoRemove = true)
        {
            StatusText = text;

            if (autoRemove)
            {
                m_StatusTextTimer.Stop();

                m_StatusTextCurrentCount = 0;

                m_StatusTextTimer.Start();

            }
        }

        #endregion Public Methods

    }
}
