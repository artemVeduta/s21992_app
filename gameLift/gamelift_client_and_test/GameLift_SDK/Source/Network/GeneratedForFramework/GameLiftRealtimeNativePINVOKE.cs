//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.0
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace GameLiftRealtimeNative {

class GameLiftRealtimeNativePINVOKE {

  /// For Mono AOT platforms compatibility
  /// This attribute can be defined in any assembly for Mono AOT to use it.
  public class MonoPInvokeCallbackAttribute : global::System.Attribute {
    public MonoPInvokeCallbackAttribute(global::System.Type t) {
    }
  }


  protected class SWIGExceptionHelper {
    public delegate void ExceptionDelegate(string message); 
    public delegate void ExceptionArgumentDelegate(string message, string paramName);

    static ExceptionDelegate applicationDelegate = new ExceptionDelegate(SetPendingApplicationException);
    static ExceptionDelegate arithmeticDelegate = new ExceptionDelegate(SetPendingArithmeticException);
    static ExceptionDelegate divideByZeroDelegate = new ExceptionDelegate(SetPendingDivideByZeroException);
    static ExceptionDelegate indexOutOfRangeDelegate = new ExceptionDelegate(SetPendingIndexOutOfRangeException);
    static ExceptionDelegate invalidCastDelegate = new ExceptionDelegate(SetPendingInvalidCastException);
    static ExceptionDelegate invalidOperationDelegate = new ExceptionDelegate(SetPendingInvalidOperationException);
    static ExceptionDelegate ioDelegate = new ExceptionDelegate(SetPendingIOException);
    static ExceptionDelegate nullReferenceDelegate = new ExceptionDelegate(SetPendingNullReferenceException);
    static ExceptionDelegate outOfMemoryDelegate = new ExceptionDelegate(SetPendingOutOfMemoryException);
    static ExceptionDelegate overflowDelegate = new ExceptionDelegate(SetPendingOverflowException);
    static ExceptionDelegate systemDelegate = new ExceptionDelegate(SetPendingSystemException);

    static ExceptionArgumentDelegate argumentDelegate = new ExceptionArgumentDelegate(SetPendingArgumentException);
    static ExceptionArgumentDelegate argumentNullDelegate = new ExceptionArgumentDelegate(SetPendingArgumentNullException);
    static ExceptionArgumentDelegate argumentOutOfRangeDelegate = new ExceptionArgumentDelegate(SetPendingArgumentOutOfRangeException);

    [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="SWIGRegisterExceptionCallbacks_GameLiftRealtimeNative")]
    public static extern void SWIGRegisterExceptionCallbacks_GameLiftRealtimeNative(
                                ExceptionDelegate applicationDelegate,
                                ExceptionDelegate arithmeticDelegate,
                                ExceptionDelegate divideByZeroDelegate, 
                                ExceptionDelegate indexOutOfRangeDelegate, 
                                ExceptionDelegate invalidCastDelegate,
                                ExceptionDelegate invalidOperationDelegate,
                                ExceptionDelegate ioDelegate,
                                ExceptionDelegate nullReferenceDelegate,
                                ExceptionDelegate outOfMemoryDelegate, 
                                ExceptionDelegate overflowDelegate, 
                                ExceptionDelegate systemExceptionDelegate);

    [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="SWIGRegisterExceptionArgumentCallbacks_GameLiftRealtimeNative")]
    public static extern void SWIGRegisterExceptionCallbacksArgument_GameLiftRealtimeNative(
                                ExceptionArgumentDelegate argumentDelegate,
                                ExceptionArgumentDelegate argumentNullDelegate,
                                ExceptionArgumentDelegate argumentOutOfRangeDelegate);

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingApplicationException(string message) {
      SWIGPendingException.Set(new global::System.ApplicationException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingArithmeticException(string message) {
      SWIGPendingException.Set(new global::System.ArithmeticException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingDivideByZeroException(string message) {
      SWIGPendingException.Set(new global::System.DivideByZeroException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingIndexOutOfRangeException(string message) {
      SWIGPendingException.Set(new global::System.IndexOutOfRangeException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingInvalidCastException(string message) {
      SWIGPendingException.Set(new global::System.InvalidCastException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingInvalidOperationException(string message) {
      SWIGPendingException.Set(new global::System.InvalidOperationException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingIOException(string message) {
      SWIGPendingException.Set(new global::System.IO.IOException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingNullReferenceException(string message) {
      SWIGPendingException.Set(new global::System.NullReferenceException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingOutOfMemoryException(string message) {
      SWIGPendingException.Set(new global::System.OutOfMemoryException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingOverflowException(string message) {
      SWIGPendingException.Set(new global::System.OverflowException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionDelegate))]
    static void SetPendingSystemException(string message) {
      SWIGPendingException.Set(new global::System.SystemException(message, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionArgumentDelegate))]
    static void SetPendingArgumentException(string message, string paramName) {
      SWIGPendingException.Set(new global::System.ArgumentException(message, paramName, SWIGPendingException.Retrieve()));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionArgumentDelegate))]
    static void SetPendingArgumentNullException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentNullException(paramName, message));
    }

    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(ExceptionArgumentDelegate))]
    static void SetPendingArgumentOutOfRangeException(string message, string paramName) {
      global::System.Exception e = SWIGPendingException.Retrieve();
      if (e != null) message = message + " Inner Exception: " + e.Message;
      SWIGPendingException.Set(new global::System.ArgumentOutOfRangeException(paramName, message));
    }

    static SWIGExceptionHelper() {
      SWIGRegisterExceptionCallbacks_GameLiftRealtimeNative(
                                applicationDelegate,
                                arithmeticDelegate,
                                divideByZeroDelegate,
                                indexOutOfRangeDelegate,
                                invalidCastDelegate,
                                invalidOperationDelegate,
                                ioDelegate,
                                nullReferenceDelegate,
                                outOfMemoryDelegate,
                                overflowDelegate,
                                systemDelegate);

      SWIGRegisterExceptionCallbacksArgument_GameLiftRealtimeNative(
                                argumentDelegate,
                                argumentNullDelegate,
                                argumentOutOfRangeDelegate);
    }
  }

  protected static SWIGExceptionHelper swigExceptionHelper = new SWIGExceptionHelper();

  public class SWIGPendingException {
    [global::System.ThreadStatic]
    private static global::System.Exception pendingException = null;
    private static int numExceptionsPending = 0;
    private static global::System.Object exceptionsLock = null;

    public static bool Pending {
      get {
        bool pending = false;
        if (numExceptionsPending > 0)
          if (pendingException != null)
            pending = true;
        return pending;
      } 
    }

    public static void Set(global::System.Exception e) {
      if (pendingException != null)
        throw new global::System.ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + pendingException.ToString() + ")", e);
      pendingException = e;
      lock(exceptionsLock) {
        numExceptionsPending++;
      }
    }

    public static global::System.Exception Retrieve() {
      global::System.Exception e = null;
      if (numExceptionsPending > 0) {
        if (pendingException != null) {
          e = pendingException;
          pendingException = null;
          lock(exceptionsLock) {
            numExceptionsPending--;
          }
        }
      }
      return e;
    }

    static SWIGPendingException() {
      exceptionsLock = new global::System.Object();
    }
  }


  protected class SWIGStringHelper {
    public delegate string SWIGStringDelegate(string message);
    static SWIGStringDelegate stringDelegate = new SWIGStringDelegate(CreateString);

    [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="SWIGRegisterStringCallback_GameLiftRealtimeNative")]
    public static extern void SWIGRegisterStringCallback_GameLiftRealtimeNative(SWIGStringDelegate stringDelegate);


    [GameLiftRealtimeNativePINVOKE.MonoPInvokeCallback(typeof(SWIGStringDelegate))]
    static string CreateString(string cString) {
      return cString;
    }

    static SWIGStringHelper() {
      SWIGRegisterStringCallback_GameLiftRealtimeNative(stringDelegate);
    }
  }

  static protected SWIGStringHelper swigStringHelper = new SWIGStringHelper();


  static GameLiftRealtimeNativePINVOKE() {
  }


  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_new_NativeByteArray")]
  public static extern global::System.IntPtr new_NativeByteArray(int jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_delete_NativeByteArray")]
  public static extern void delete_NativeByteArray(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_NativeByteArray_getitem")]
  public static extern byte NativeByteArray_getitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_NativeByteArray_setitem")]
  public static extern void NativeByteArray_setitem(global::System.Runtime.InteropServices.HandleRef jarg1, int jarg2, byte jarg3);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_NativeByteArray_cast")]
  public static extern global::System.IntPtr NativeByteArray_cast(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_NativeByteArray_frompointer")]
  public static extern global::System.IntPtr NativeByteArray_frompointer(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_delete_SendEncryptedCallback")]
  public static extern void delete_SendEncryptedCallback(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_SendEncryptedCallback_sendEncrypted")]
  public static extern void SendEncryptedCallback_sendEncrypted(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_new_SendEncryptedCallback")]
  public static extern global::System.IntPtr new_SendEncryptedCallback();

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_SendEncryptedCallback_director_connect")]
  public static extern void SendEncryptedCallback_director_connect(global::System.Runtime.InteropServices.HandleRef jarg1, SendEncryptedCallback.SwigDelegateSendEncryptedCallback_0_Dispatcher delegate0_dispatcher, global::System.IntPtr delegate0gcHandlePtr);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_new_DTLSConnection_Result__SWIG_0")]
  public static extern global::System.IntPtr new_DTLSConnection_Result__SWIG_0(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_delete_DTLSConnection_Result")]
  public static extern void delete_DTLSConnection_Result(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_Result_get_type")]
  public static extern int DTLSConnection_Result_get_type(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_Result_get_message")]
  public static extern global::System.IntPtr DTLSConnection_Result_get_message(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_Result_get_length")]
  public static extern uint DTLSConnection_Result_get_length(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_Result_get_info")]
  public static extern string DTLSConnection_Result_get_info(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_Result_get_code")]
  public static extern int DTLSConnection_Result_get_code(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_new_DTLSConnection")]
  public static extern global::System.IntPtr new_DTLSConnection(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_delete_DTLSConnection")]
  public static extern void delete_DTLSConnection(global::System.Runtime.InteropServices.HandleRef jarg1);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_open")]
  public static extern global::System.IntPtr DTLSConnection_open(global::System.Runtime.InteropServices.HandleRef jarg1, string jarg2, string jarg3, uint jarg4);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_send")]
  public static extern global::System.IntPtr DTLSConnection_send(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_receive_message")]
  public static extern global::System.IntPtr DTLSConnection_receive_message(global::System.Runtime.InteropServices.HandleRef jarg1, global::System.Runtime.InteropServices.HandleRef jarg2, uint jarg3);

  [global::System.Runtime.InteropServices.DllImport("__Internal", EntryPoint="CSharp_GameLiftRealtimeNative_DTLSConnection_close")]
  public static extern global::System.IntPtr DTLSConnection_close(global::System.Runtime.InteropServices.HandleRef jarg1);
}

}
