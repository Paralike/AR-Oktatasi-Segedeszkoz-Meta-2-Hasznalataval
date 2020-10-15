// SolvePnPDll.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "SolvePnPDll.h"


// This is an example of an exported variable
SOLVEPNPDLL_API int nSolvePnPDll=0;

// This is an example of an exported function.
SOLVEPNPDLL_API int fnSolvePnPDll(void)
{
    return 2;
}

// This is the constructor of a class that has been exported.
CSolvePnPDll::CSolvePnPDll()
{
    return;
}

int CSolvePnPDll::SolvePnp() {

	return 3;
}

double Add(double a, double b)
{
	return a + b;
}
