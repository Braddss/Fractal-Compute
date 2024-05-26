#ifndef JULIA_SET_CGINC
#define JULIA_SET_CGINC

#include "ComplexNumbers.cginc"

int SampleJulia(Complex current, Complex complex, float cap, int iterations)
{
    int limit = iterations;
    for (int i = 0; i < limit; i++)
    {
        if (CMaginute(complex) > cap)
        {
            return i;
        }
        complex = CAdd(CMult(complex, complex), current);
    }
    
    return -1;
}

#endif //JULIA_SET_CGINC