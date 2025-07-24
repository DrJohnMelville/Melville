using System;
using Melville.Hacks;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy;

public class FunctionFactoryImplementation
{
    private readonly IBindingRequest bindingRequest;
    private readonly Type desiredType;

    public FunctionFactoryImplementation(IBindingRequest bindingRequest,
        Type desiredType)
    {
        this.bindingRequest = bindingRequest;
        this.desiredType = desiredType;
    }

    public object? CreateTargetObject(params object[] parameters) =>
        bindingRequest.IocService.Get(bindingRequest.CreateSubRequest(desiredType, parameters));

    public TResult Call<TResult>() =>
        (TResult)CreateTargetObject()!;

    public TResult Call<T1, TResult>(T1 a1) =>
        (TResult)CreateTargetObject(a1!)!;

    public TResult Call<T1, T2, TResult>(T1 a1, T2 a2) =>
        (TResult)CreateTargetObject(a1!, a2!)!;

    public TResult Call<T1, T2, T3, TResult>(T1 a1, T2 a2, T3 a3) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!)!;

    public TResult Call<T1, T2, T3, T4, TResult>(T1 a1, T2 a2, T3 a3, T4 a4) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!)!;

    public TResult Call<T1, T2, T3, T4, T5, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!)!;

    public TResult
        Call<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7,
        T8 a8, T9 a9) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6,
        T7 a7, T8 a8, T9 a9, T10 a10) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6,
        T7 a7, T8 a8, T9 a9, T10 a10, T11 a11) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!, a11!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5,
        T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!, a11!, a12!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(T1 a1, T2 a2, T3 a3, T4 a4,
        T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!, a11!, a12!, a13!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(T1 a1, T2 a2, T3 a3,
        T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!, a11!, a12!, a13!, a14!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(T1 a1, T2 a2, T3 a3,
        T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!, a11!, a12!, a13!, a14!, a15!)!;

    public TResult Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(T1 a1, T2 a2,
        T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15,
        T16 a16) =>
        (TResult)CreateTargetObject(a1!, a2!, a3!, a4!, a5!, a6!, a7!, a8!, a9!, a10!, a11!, a12!, a13!, a14!, a15!,
            a16!)!;
}