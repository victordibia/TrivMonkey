using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TrivMonkey
{

    public class CategoryDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public CategoryDataContext(string connectionString)
            : base(connectionString)
        { }
        // Specify a single table for the to-do items.
        public Table<CategoryItem> CategoryItems;
    }

    public class SubCategoryDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public SubCategoryDataContext(string connectionString)
            : base(connectionString)
        { }
        // Specify a single table for the to-do items.
        public Table<SubCategoryItem> SubCategoryItems;
    }

    public class QuestionDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public QuestionDataContext(string connectionString)
            : base(connectionString)
        { }
        // Specify a single table for the to-do items.
        public Table<QuestionItem> QuestionItems;
    }
    

    public class ToDoDataContext : DataContext
    { 

        // Pass the connection string to the base class.
        public ToDoDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a single table for the to-do items.
        public Table<ToDoItem> ToDoItems;
        public Table<SubCategoryItem> SubCategoryItems;
        public Table<QuestionItem> QuestionItems;
        public Table<CategoryItem> CategoryItems;
        public Table<SettingItem> SettingItems;

    }

    [Table]
    public class ToDoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _toDoItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ToDoItemId
        {
            get
            {
                return _toDoItemId;
            }
            set
            {
                if (_toDoItemId != value)
                {
                    NotifyPropertyChanging("ToDoItemId");
                    _toDoItemId = value;
                    NotifyPropertyChanged("ToDoItemId");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _itemName;

        [Column]
        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                if (_itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    _itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        // Define completion value: private field, public property and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class SettingItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("id");
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _lifetimemcoins;

        [Column]
        public string lifetimemcoins
        {
            get
            {
                return _lifetimemcoins;
            }
            set
            {
                if (_lifetimemcoins != value)
                {
                    NotifyPropertyChanging("lifetimemcoins");
                    _lifetimemcoins = value;
                    NotifyPropertyChanged("lifetimemcoins");
                }
            }
        }
        // Define item name: private field, public property and database column.
        private string _themecolor;

        [Column]
        public string themecolor
        {
            get
            {
                return _themecolor;
            }
            set
            {
                if (_themecolor != value)
                {
                    NotifyPropertyChanging("themecolor");
                    _themecolor = value;
                    NotifyPropertyChanged("themecolor");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _displayname;

        [Column]
        public string displayname
        {
            get
            {
                return _displayname;
            }
            set
            {
                if (_displayname != value)
                {
                    NotifyPropertyChanging("displayname");
                    _displayname = value;
                    NotifyPropertyChanged("displayname");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _vibration;

        [Column]
        public string vibration
        {
            get
            {
                return _vibration;
            }
            set
            {
                if (_vibration != value)
                {
                    NotifyPropertyChanging("vibration");
                    _vibration = value;
                    NotifyPropertyChanged("vibration");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _highscore;

        [Column]
        public string highscore
        {
            get
            {
                return _highscore;
            }
            set
            {
                if (_highscore != value)
                {
                    NotifyPropertyChanging("highscore");
                    _highscore = value;
                    NotifyPropertyChanged("highscore");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _gamesound;

        [Column]
        public string gamesound
        {
            get
            {
                return _gamesound;
            }
            set
            {
                if (_gamesound != value)
                {
                    NotifyPropertyChanging("gamesound");
                    _gamesound = value;
                    NotifyPropertyChanged("gamesound");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _itemsunlocked;

        [Column]
        public string itemsunlocked
        {
            get
            {
                return _itemsunlocked;
            }
            set
            {
                if (_itemsunlocked != value)
                {
                    NotifyPropertyChanging("itemsunlocked");
                    _itemsunlocked = value;
                    NotifyPropertyChanged("itemsunlocked");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _gamesplayed;

        [Column]
        public string gamesplayed
        {
            get
            {
                return _gamesplayed;
            }
            set
            {
                if (_gamesplayed != value)
                {
                    NotifyPropertyChanging("gamesplayed");
                    _gamesplayed = value;
                    NotifyPropertyChanged("gamesplayed");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _playtime;

        [Column]
        public string playtime
        {
            get
            {
                return _playtime;
            }
            set
            {
                if (_playtime != value)
                {
                    NotifyPropertyChanging("playtime");
                    _playtime = value;
                    NotifyPropertyChanged("playtime");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _playtimemod;

        [Column]
        public string playtimemod
        {
            get
            {
                return _playtimemod;
            }
            set
            {
                if (_playtimemod != value)
                {
                    NotifyPropertyChanging("playtimemod");
                    _playtimemod = value;
                    NotifyPropertyChanged("playtimemod");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _score;

        [Column]
        public string score
        {
            get
            {
                return _score;
            }
            set
            {
                if (_score != value)
                {
                    NotifyPropertyChanging("score");
                    _score = value;
                    NotifyPropertyChanged("score");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _mcoins;

        [Column]
        public string mcoins
        {
            get
            {
                return _mcoins;
            }
            set
            {
                if (_mcoins != value)
                {
                    NotifyPropertyChanging("mcoins");
                    _mcoins = value;
                    NotifyPropertyChanged("mcoins");
                }
            }
        }


        // Define completion value: private field, public property and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }


    [Table]
    public class CategoryItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("id");
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _value;

        [Column]
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    NotifyPropertyChanging("value");
                    _value = value;
                    NotifyPropertyChanged("value");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _title;

        [Column]
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("title");
                    _title = value;
                    NotifyPropertyChanged("title");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _lockstatus;

        [Column]
        public string lockstatus
        {
            get
            {
                return _lockstatus;
            }
            set
            {
                if (_lockstatus != value)
                {
                    NotifyPropertyChanging("lockstatus");
                    _lockstatus = value;
                    NotifyPropertyChanged("lockstatus");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _mcoins;

        [Column]
        public string mcoins
        {
            get
            {
                return _mcoins;
            }
            set
            {
                if (_mcoins != value)
                {
                    NotifyPropertyChanging("mcoins");
                    _mcoins = value;
                    NotifyPropertyChanged("mcoins");
                }
            }
        }

              // Define item name: private field, public property and database column.
        private string _imagelink;

        [Column]
        public string imagelink
        {
            get
            {
                return _imagelink;
            }
            set
            {
                if (_imagelink != value)
                {
                    NotifyPropertyChanging("imagelink");
                    _imagelink = value;
                    NotifyPropertyChanged("imagelink");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _thumbnaillink;

        [Column]
        public string thumbnaillink
        {
            get
            {
                return _thumbnaillink;
            }
            set
            {
                if (_thumbnaillink != value)
                {
                    NotifyPropertyChanging("thumbnaillink");
                    _thumbnaillink = value;
                    NotifyPropertyChanged("thumbnaillink");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _description;

        [Column]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    NotifyPropertyChanging("description");
                    _description = value;
                    NotifyPropertyChanged("description");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _created_at;

        [Column]
        public string created_at
        {
            get
            {
                return _created_at;
            }
            set
            {
                if (_created_at != value)
                {
                    NotifyPropertyChanging("created_at");
                    _created_at = value;
                    NotifyPropertyChanged("created_at");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _updated_at;

        [Column]
        public string updated_at
        {
            get
            {
                return _updated_at;
            }
            set
            {
                if (_updated_at != value)
                {
                    NotifyPropertyChanging("updated_at");
                    _updated_at = value;
                    NotifyPropertyChanged("updated_at");
                }
            }
        }

        //===========================================================================



        // Define completion value: private field, public property and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }


    [Table]
    public class SubCategoryItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("id");
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _value;

        [Column]
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    NotifyPropertyChanging("value");
                    _value = value;
                    NotifyPropertyChanged("value");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _highscore;

        [Column]
        public string highscore
        {
            get
            {
                return _highscore;
            }
            set
            {
                if (_highscore != value)
                {
                    NotifyPropertyChanging("highscore");
                    _highscore = value;
                    NotifyPropertyChanged("highscore");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _timesplayed;

        [Column]
        public string timesplayed
        {
            get
            {
                return _timesplayed;
            }
            set
            {
                if (_timesplayed != value)
                {
                    NotifyPropertyChanging("timesplayed");
                    _timesplayed = value;
                    NotifyPropertyChanged("timesplayed");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _lastquestion;

        [Column]
        public string lastquestion
        {
            get
            {
                return _lastquestion;
            }
            set
            {
                if (_lastquestion != value)
                {
                    NotifyPropertyChanging("lastquestion");
                    _lastquestion = value;
                    NotifyPropertyChanged("lastquestion");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _title;

        [Column]
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    NotifyPropertyChanging("title");
                    _title = value;
                    NotifyPropertyChanged("title");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _category;

        [Column]
        public string category
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != value)
                {
                    NotifyPropertyChanging("category");
                    _category = value;
                    NotifyPropertyChanged("category");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _featured;

        [Column]
        public string featured
        {
            get
            {
                return _featured;
            }
            set
            {
                if (_featured != value)
                {
                    NotifyPropertyChanging("featured");
                    _featured = value;
                    NotifyPropertyChanged("featured");
                }
            }
        }



        // Define item name: private field, public property and database column.
        private string _imagelink;

        [Column]
        public string imagelink
        {
            get
            {
                return _imagelink;
            }
            set
            {
                if (_imagelink != value)
                {
                    NotifyPropertyChanging("imagelink");
                    _imagelink = value;
                    NotifyPropertyChanged("imagelink");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _thumbnaillink;

        [Column]
        public string thumbnaillink
        {
            get
            {
                return _thumbnaillink;
            }
            set
            {
                if (_thumbnaillink != value)
                {
                    NotifyPropertyChanging("thumbnaillink");
                    _thumbnaillink = value;
                    NotifyPropertyChanged("thumbnaillink");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _description;

        [Column]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    NotifyPropertyChanging("description");
                    _description = value;
                    NotifyPropertyChanged("description");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _created_at;

        [Column]
        public string created_at
        {
            get
            {
                return _created_at;
            }
            set
            {
                if (_created_at != value)
                {
                    NotifyPropertyChanging("created_at");
                    _created_at = value;
                    NotifyPropertyChanged("created_at");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _updated_at;

        [Column]
        public string updated_at
        {
            get
            {
                return _updated_at;
            }
            set
            {
                if (_updated_at != value)
                {
                    NotifyPropertyChanging("updated_at");
                    _updated_at = value;
                    NotifyPropertyChanged("updated_at");
                }
            }
        }

        //===========================================================================



        // Define completion value: private field, public property and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table]
    public class QuestionItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("id");
                    _id = value;
                    NotifyPropertyChanged("id");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _user_id;

        [Column]
        public string user_id
        {
            get
            {
                return _user_id;
            }
            set
            {
                if (_user_id != value)
                {
                    NotifyPropertyChanging("user_id");
                    _user_id = value;
                    NotifyPropertyChanged("user_id");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _content;

        [Column]
        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != value)
                {
                    NotifyPropertyChanging("content");
                    _content = value;
                    NotifyPropertyChanged("content");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _level;

        [Column]
        public string level
        {
            get
            {
                return _level;
            }
            set
            {
                if (_level != value)
                {
                    NotifyPropertyChanging("level");
                    _level = value;
                    NotifyPropertyChanged("level");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _optiona;

        [Column]
        public string optiona
        {
            get
            {
                return _optiona;
            }
            set
            {
                if (_optiona != value)
                {
                    NotifyPropertyChanging("optiona");
                    _optiona = value;
                    NotifyPropertyChanged("optiona");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _optionb;

        [Column]
        public string optionb
        {
            get
            {
                return _optionb;
            }
            set
            {
                if (_optionb != value)
                {
                    NotifyPropertyChanging("optionb");
                    _optionb = value;
                    NotifyPropertyChanged("optionb");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _optionc;

        [Column]
        public string optionc
        {
            get
            {
                return _optionc;
            }
            set
            {
                if (_optionc != value)
                {
                    NotifyPropertyChanging("optionc");
                    _optionc = value;
                    NotifyPropertyChanged("optionc");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _optiond;

        [Column]
        public string optiond
        {
            get
            {
                return _optiond;
            }
            set
            {
                if (_optiond != value)
                {
                    NotifyPropertyChanging("optiond");
                    _optiond = value;
                    NotifyPropertyChanged("optiond");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _correct;

        [Column]
        public string correct
        {
            get
            {
                return _correct;
            }
            set
            {
                if (_correct != value)
                {
                    NotifyPropertyChanging("correct");
                    _correct = value;
                    NotifyPropertyChanged("correct");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _categoryid;

        [Column]
        public string categoryid
        {
            get
            {
                return _categoryid;
            }
            set
            {
                if (_categoryid != value)
                {
                    NotifyPropertyChanging("categoryid");
                    _categoryid = value;
                    NotifyPropertyChanged("categoryid");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _subcategoryid;

        [Column]
        public string subcategoryid
        {
            get
            {
                return _subcategoryid;
            }
            set
            {
                if (_subcategoryid != value)
                {
                    NotifyPropertyChanging("subcategoryid");
                    _subcategoryid = value;
                    NotifyPropertyChanged("subcategoryid");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _explanation;

        [Column]
        public string explanation
        {
            get
            {
                return _explanation;
            }
            set
            {
                if (_explanation != value)
                {
                    NotifyPropertyChanging("explanation");
                    _explanation = value;
                    NotifyPropertyChanged("explanation");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _link;

        [Column]
        public string link
        {
            get
            {
                return _link;
            }
            set
            {
                if (_link != value)
                {
                    NotifyPropertyChanging("link");
                    _link = value;
                    NotifyPropertyChanged("link");
                }
            }
        }

        // Define item name: private field, public property and database column.
        private string _created_at;
        [Column]
        public string created_at
        {
            get
            {
                return _created_at;
            }
            set
            {
                if (_created_at != value)
                {
                    NotifyPropertyChanging("created_at");
                    _created_at = value;
                    NotifyPropertyChanged("created_at");
                }
            }
        }


        // Define item name: private field, public property and database column.
        private string _updated_at;

        [Column]
        public string updated_at
        {
            get
            {
                return _updated_at;
            }
            set
            {
                if (_updated_at != value)
                {
                    NotifyPropertyChanging("updated_at");
                    _updated_at = value;
                    NotifyPropertyChanged("updated_at");
                }
            }
        }

        //===========================================================================

        // Define completion value: private field, public property and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

     
}
