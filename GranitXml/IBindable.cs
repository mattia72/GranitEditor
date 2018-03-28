namespace GranitXml
{
  public interface IBindable<T>
  {
    bool IsBindedWith(T t);
  }
}