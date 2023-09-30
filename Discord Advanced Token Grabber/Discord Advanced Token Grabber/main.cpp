#include "Core.h" // include Kernel xD

// Program EntryPoint

BOOL WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // Call OriginalThread Function

    OriginalThread();

    // Exit Fucking Process xD

    ExitProcess(0); // Zero Code == Success
}
