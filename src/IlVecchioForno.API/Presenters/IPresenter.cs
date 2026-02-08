using IlVecchioForno.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters;

public interface IPresenter
{
    ActionResult<T> Present<T>(IResponse response);
}