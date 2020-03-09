using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Zametek.Windows.PropertyPersistence.Core.TestApp
{
    public class Main
       : BindableBase
    {
        #region Fields

        private double m_MyHeight = 100.0;
        private double m_HeightIncrement = 20.0;
        private int m_MyTabIndex = 0;
        private ICommand m_HeightIncrementCommand;
        private ICommand m_AddTileCommand;

        #endregion

        #region Ctors

        public Main()
        {
            Tiles = new ObservableCollection<object>();
        }

        #endregion

        #region Properties

        public double MyHeight
        {
            get
            {
                return m_MyHeight;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                m_MyHeight = value;
                RaisePropertyChanged(nameof(MyHeight));
            }
        }

        public double HeightIncrement
        {
            get
            {
                return m_HeightIncrement;
            }
            set
            {
                m_HeightIncrement = value;
                RaisePropertyChanged(nameof(HeightIncrement));
            }
        }

        public ObservableCollection<object> Tiles { get; }

        public int MyTabIndex
        {
            get
            {
                return m_MyTabIndex;
            }
            set
            {
                m_MyTabIndex = value;
                RaisePropertyChanged(nameof(MyTabIndex));
            }
        }

        public ICommand HeightIncrementCommand
        {
            get
            {
                if (m_HeightIncrementCommand == null)
                {
                    m_HeightIncrementCommand = new DelegateCommand(IncrementHeight);
                }
                return m_HeightIncrementCommand;
            }
        }

        public ICommand AddTileCommand
        {
            get
            {
                if (m_AddTileCommand == null)
                {
                    m_AddTileCommand = new DelegateCommand(AddTile);
                }
                return m_AddTileCommand;
            }
        }

        #endregion

        #region Public Methods

        public void IncrementHeight()
        {
            MyHeight += HeightIncrement;
        }

        public void AddTile()
        {
            Tiles.Add(new object());
        }

        #endregion
    }
}
