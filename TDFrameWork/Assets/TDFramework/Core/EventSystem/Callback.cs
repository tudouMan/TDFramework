
namespace TDFramework.EventSystem
{
    public delegate void CallbackE();
    public delegate void CallbackE<T>(T arg1);
    public delegate void CallbackE<T, U>(T arg1, U arg2);
    public delegate void CallbackE<T, U, V>(T arg1, U arg2, V arg3);
    public delegate void CallbackE<T, U, V, X>(T arg1, U arg2, V arg3, X arg4);
    public delegate void CallbackE<T, U, V, X, Y>(T arg1, U arg2, V arg3, X arg4, Y arg5);
    public delegate void CallbackE<T, U, V, X, Y, Z>(T arg1, U arg2, V arg3, X arg4, Y arg5, Z arg6);
}



