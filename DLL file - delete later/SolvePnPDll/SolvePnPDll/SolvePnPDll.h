// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the SOLVEPNPDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// SOLVEPNPDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef SOLVEPNPDLL_EXPORTS
#define SOLVEPNPDLL_API __declspec(dllexport)
#else
#define SOLVEPNPDLL_API __declspec(dllimport)
#endif

// This class is exported from the dll
class SOLVEPNPDLL_API CSolvePnPDll {
public:
	CSolvePnPDll(void);
	// TODO: add your methods here.
	int SolvePnp(void);
};

extern SOLVEPNPDLL_API int nSolvePnPDll;

SOLVEPNPDLL_API int fnSolvePnPDll(void);

extern "C" __declspec(dllexport) double Add(double a, double b);