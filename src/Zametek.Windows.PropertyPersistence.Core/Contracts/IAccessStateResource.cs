
namespace Zametek.Wpf.Core
{
    public interface IAccessStateResource<TState>
    {
        TState Load();

        void Save(TState state);
    }
}
