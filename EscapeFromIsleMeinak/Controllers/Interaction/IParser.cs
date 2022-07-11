using EscapeFromIsleMeinak;
using EscapeFromIsleMeinak.Components;

namespace EscapeFromIsleMeinak
{
    public interface IParser
    {
        bool Parse(Ctx ctx, InputBundle input);
    }
}
