namespace GranitXMLEditor
{
  internal interface IBindable<T>
  {
    bool IsBindedWith(T t);
  }
}