// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Bryan Ross">
//   (c) 2012 Bryan Ross
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RegExpress
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public partial class MainWindow : INotifyPropertyChanged
    {
        private string message;

        private Brush messageBrush;

        private string expression;

        private string input;

        private RegexOptions options;

        private Match selectedMatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.Matches = new ObservableCollection<Match>();
            this.Groups = new ObservableCollection<Group>();
            this.Message = string.Empty;
            this.MessageBrush = Brushes.Green;

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public ObservableCollection<Match> Matches { get; private set; }

        public string Message
        {
            get { return this.message; }
            private set { this.message = value; this.OnPropertyChanged("Message"); }
        }

        public Brush MessageBrush
        {
            get { return this.messageBrush; }
            private set { this.messageBrush = value; this.OnPropertyChanged("MessageBrush"); }
        }

        public string Expression
        {
            get { return this.expression; }
            set { this.expression = value; this.OnPropertyChanged("Expression"); }
        }

        public string Input
        {
            get { return this.input; }
            set { this.input = value; this.OnPropertyChanged("Input"); }
        }

        public Match SelectedMatch
        {
            get { return this.selectedMatch; }
            set
            {
                this.selectedMatch = value; 
                this.OnPropertyChanged("SelectedMatch");
                this.Groups.Clear();
                foreach (Group group in value.Groups)
                {
                    this.Groups.Add(group);
                }
            }
        }

        public ObservableCollection<Group> Groups { get; private set; }

        private void MatchButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Expression))
            {
                this.Message = "No expression.";
                this.IsError = true;
                return;
            }

            this.Input = string.IsNullOrWhiteSpace(this.Input) ? string.Empty : this.Input;

            var matches = Regex.Matches(this.Input, this.Expression, this.Options);
            this.Matches.Clear();
            foreach (Match match in matches)
            {
                this.Matches.Add(match);
            }

            if (matches.Count > 0)
            {
                this.SelectedMatch = matches.Cast<Match>().First();
            }

            this.Message = matches.Count > 0 ? "Match" : "No Match";
            this.IsError = matches.Count == 0;
        }

        protected bool IsError
        {
            set
            {
                this.MessageBrush = value ? Brushes.Red : Brushes.Green;
            }
        }

        private RegexOptions Options
        {
            get { return options; }
        }

        #region Options

        public bool IgnoreCase
        {
            get { return this.CheckFlag(RegexOptions.IgnoreCase); }
            set { this.SetFlag(RegexOptions.IgnoreCase, value); this.OnPropertyChanged("IgnoreCase"); }
        }

        public bool Multiline
        {
            get { return this.CheckFlag(RegexOptions.Multiline); }
            set { this.SetFlag(RegexOptions.Multiline, value); this.OnPropertyChanged("Multiline"); }
        }

        public bool ExplicitCapture
        {
            get { return this.CheckFlag(RegexOptions.ExplicitCapture); }
            set { this.SetFlag(RegexOptions.ExplicitCapture, value); this.OnPropertyChanged("ExplicitCapture"); }
        }

        public bool Singleline
        {
            get { return this.CheckFlag(RegexOptions.Singleline); }
            set { this.SetFlag(RegexOptions.Singleline, value); this.OnPropertyChanged("Singleline"); }
        }

        public bool IgnorePatternWhitespace
        {
            get { return this.CheckFlag(RegexOptions.IgnorePatternWhitespace); }
            set { this.SetFlag(RegexOptions.IgnorePatternWhitespace, value); this.OnPropertyChanged("IgnorePatternWhitespace"); }
        }

        public bool RightToLeft
        {
            get { return this.CheckFlag(RegexOptions.RightToLeft); }
            set { this.SetFlag(RegexOptions.RightToLeft, value); this.OnPropertyChanged("RightToLeft"); }
        }

        public bool ECMAScript
        {
            get { return this.CheckFlag(RegexOptions.ECMAScript); }
            set { this.SetFlag(RegexOptions.ECMAScript, value); this.OnPropertyChanged("ECMAScript"); }
        }

        public bool CultureInvariant
        {
            get { return this.CheckFlag(RegexOptions.CultureInvariant); }
            set { this.SetFlag(RegexOptions.CultureInvariant, value); this.OnPropertyChanged("CultureInvariant"); }
        }

        #endregion

        private void SetFlag(RegexOptions flag, bool value)
        {
            this.options = value ? this.options | flag : this.options & ~flag;
        }

        private bool CheckFlag(RegexOptions flag)
        {
            return (this.options & flag) == flag;
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global
    }
}
