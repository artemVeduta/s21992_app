%module(directors="1") GameLiftRealtimeNative

%include <typemaps.i>
%include <carrays.i>
%include <stl.i>

%array_class(unsigned char, NativeByteArray);

%{
/* Includes the header in the wrapper code */
#include "../include/DTLSConnection.h"

%}

%feature("director") SendEncryptedCallback;

%include "../include/DTLSConnection.h"
