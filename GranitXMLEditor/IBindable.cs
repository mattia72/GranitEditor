namespace GranitEditor
{
  internal interface IBindable<T>
  {
    bool IsBindedWith(T t);
  }
}