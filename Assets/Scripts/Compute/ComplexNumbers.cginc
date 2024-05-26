#ifndef COMPLEX_NUMBERS_CGINC
#define COMPLEX_NUMBERS_CGINC

struct Complex
{
    float real;
    float imaginary;
};

Complex CAdd(Complex one, Complex two)
{
    Complex result;
    result.real = one.real + two.real;
    result.imaginary = one.imaginary + two.imaginary;
    return result;
}

Complex CSubtract(Complex one, Complex two)
{
    Complex result;
    result.real = one.real - two.real;
    result.imaginary = one.imaginary - two.imaginary;
    return result;
}

Complex CMult(Complex one, Complex two)
{
    Complex result;
    result.real = one.real * two.real - one.imaginary * two.imaginary;
    result.imaginary = one.real * two.imaginary + one.imaginary * two.real;
    return result;
}

float CMaginute(Complex one)
{
    return length(float2(one.real, one.imaginary));
}

float CDistance(Complex one, Complex two)
{
    return distance(float2(one.real, one.imaginary), float2(two.real, two.imaginary));

}

#endif //COMPLEX_NUMBERS_CGINC