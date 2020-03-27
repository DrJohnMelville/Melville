namespace Melville.CurveFit.MpFit
{
    public class MpPar
    {
        public int isFixed;        /* 1 = fixed; 0 = free */
        public int[] limited = new int[2];   /* 1 = low/upper limit; 0 = no limit */
        public double[] limits = new double[2]; /* lower/upper limit boundary value */

        public string parname;    /* Name of parameter, or 0 for none */
        public double step;      /* Step size for finite difference */
        public double relstep;   /* Relative step size for finite difference */
        public int side;         /* Sidedness of finite difference derivative 
		                0 - one-sided derivative computed automatically
		                1 - one-sided derivative (f(x+h) - f(x)  )/h
		               -1 - one-sided derivative (f(x)   - f(x-h))/h
		                2 - two-sided derivative (f(x+h) - f(x-h))/(2*h) 
			        3 - user-computed analytical derivatives
		            */
        public int deriv_debug;  /* Derivative debug mode: 1 = Yes; 0 = No;

                               If yes, compute both analytical and numerical
                               derivatives and print them to the console for
                               comparison.

		               NOTE: when debugging, do *not* set side = 3,
		               but rather to the kind of numerical derivative
		               you want to compare the user-analytical one to
		               (0, 1, -1, or 2).
		            */
        public double deriv_reltol; /* Relative tolerance for derivative debug printout */
        public double deriv_abstol; /* Absolute tolerance for derivative debug printout */
    }
}