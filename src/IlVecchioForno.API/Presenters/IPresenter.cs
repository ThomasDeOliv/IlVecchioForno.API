using IlVecchioForno.API.Resources;

namespace IlVecchioForno.API.Presenters;

public interface IPresenter<in TResponse, out TResource>
    where TResource : IResource
{
    TResource Present(TResponse response);
}