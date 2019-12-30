using System.Threading.Tasks;

namespace ComplexTypeDemo.ShareInterface
{
    public interface ICreateRecord : Orleans.IGrainWithIntegerKey
    {
        Task<ComplexCollection> CreateRandomData();
    }
}
