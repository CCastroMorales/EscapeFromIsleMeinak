using MeinakEsc;
using MeinakEsc.Components;

namespace EscapeFromIsleMeinak
{
    public interface IParser
    {
        bool Parse(Ctx ctx, InputBundle input);
    }
}
