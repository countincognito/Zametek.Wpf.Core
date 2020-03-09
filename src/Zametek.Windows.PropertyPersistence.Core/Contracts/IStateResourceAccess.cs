
namespace Zametek.Wpf.Core
{
    public interface IStateResourceAccess<TState>
    {
        TState Load();

        void Save(TState state);
    }
}
