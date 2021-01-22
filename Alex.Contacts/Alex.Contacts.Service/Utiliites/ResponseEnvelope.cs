using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Alex.Contacts.Service.Utiliites
{
  public class ResponseEnvelope
  {
    private readonly List<Error> _Errors;

    private ResponseEnvelope(int statusCode)
    {
      this.StatusCode = statusCode;
      this._Errors = new List<Error>();
    }
    private ResponseEnvelope(int statusCode, List<Error> errors)
      : this(statusCode) => this._Errors = errors;

    private ResponseEnvelope(int statusCode, object result)
      : this(statusCode) => this.Result = result;

    public object Result { get; }
    public int StatusCode { get; }
    public IEnumerable<Error> Errors => this._Errors.ToList();

    public void AddError(string title, string errorMessage) =>
      this._Errors.Add(new Error(title, errorMessage));

    public static object BadRequest(ModelStateDictionary modelState)
    {
      var errors = modelState
        .SelectMany(x => x.Value.Errors, (entry, errors) => new Error(entry.Key, errors.ErrorMessage))
        .ToList();

      return FromErrors((int)HttpStatusCode.BadRequest, errors);
    }

    public static object Created(object result) =>
      FromResult((int)HttpStatusCode.Created, result);


    public static ResponseEnvelope FromError(int statusCode, string errorTitle, string errorMessage)
    {
      var envelope = new ResponseEnvelope(statusCode);
      envelope.AddError(errorTitle, errorMessage);

      return envelope;
    }

    public static object UnprocessableEntity(ModelStateDictionary modelState)
    {
      _ = modelState ?? throw new ArgumentNullException(nameof(modelState));

      var errors = modelState
        .SelectMany(x => x.Value.Errors, (entry, error) => new Error(entry.Key, error.ErrorMessage))
        .ToList();

      return FromErrors((int)HttpStatusCode.UnprocessableEntity, errors);
    }

    public static object Ok(object result)
    {
      _ = result ?? throw new ArgumentNullException(nameof(result));

      return FromResult((int)HttpStatusCode.OK, result);
    }

    public static object NotFound() =>
      FromError((int)HttpStatusCode.NotFound, 
        Properties.Resources.ResourceNotFoundTitle, 
        Properties.Resources.ResourceNotFoundeMessage);

    public static object InternalServerError()
    {
      return new ResponseEnvelope((int)HttpStatusCode.InternalServerError,
        Properties.Resources.InternalServerErrorMessage);
    }

    private static ResponseEnvelope FromResult(int statusCode, object result)
    {
      _ = result ?? throw new ArgumentNullException(nameof(result));

      return new ResponseEnvelope(statusCode, result);
    }
    private static ResponseEnvelope FromErrors(int statusCode, List<Error> errors)
    {
      _ = errors ?? throw new ArgumentNullException(nameof(errors));

      if (!errors.Any())
      {
        throw new ArgumentException("You have to provide errors.");
      }
      var errorList = errors.ToList();

      return new ResponseEnvelope(statusCode, errorList);
    }
  }
}
