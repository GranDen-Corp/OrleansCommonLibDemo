using System.Threading.Tasks;
using ComplexTypeDemo.ShareInterface;

namespace ComplexTypeDemo.Grain.ObjGenerator
{
    public interface ICreateMyType
    {
        Task<MyType> CreateMyTypeObj();
    }
}
