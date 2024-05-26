#ifndef MANDELBROT_SET_CGINC
#define MANDELBROT_SET_CGINC

#include "ComplexNumbers.cginc"

int SampleMandelBrot(Complex input, float cap, int iterations)
{
    Complex complex;
    complex.real = 0;
    complex.imaginary = 0;
    int limit = iterations;
    for (int i = 0; i < limit; i++)
    {
        if (CMaginute(complex) > cap)
        {
            return i;
        }
        
        complex = CAdd(CMult(complex, complex), input);
    }
    
    return -1;
}

#endif //MANDELBROT_SET_CGINC