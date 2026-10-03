namespace GalaSoft.MvvmLight { public class ViewModelBase : System.ComponentModel.INotifyPropertyChanged { public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged; protected void RaisePropertyChanged(string n) { if(PropertyChanged!=null)PropertyChanged(this,new System.ComponentModel.PropertyChangedEventArgs(n)); } } }
namespace System.Windows.Media {public class ImageSource {}}
namespace System.Windows.Media.Imaging {public class BitmapImage:System.Windows.Media.ImageSource {public System.Uri UriSource;public BitmapImage(System.Uri u){UriSource=u;}}}
namespace SQLite {public class IgnoreAttribute:System.Attribute {}}
namespace Newtonsoft.Json.Linq {public class JObject:System.Collections.Generic.List<object> {public static JObject Parse(string s){throw new System.Exception("JSON outside image tests");}}}
