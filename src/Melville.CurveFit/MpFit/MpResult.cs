namespace Melville.CurveFit.MpFit
{
    public class MpResult
    {
        public double bestnorm;     /* Final chi^2 */
        public double orignorm;     /* Starting value of chi^2 */
        public int niter;           /* Number of iterations */
        public int nfev;            /* Number of function evaluations */
        public int status;          /* Fitting status code */

        public int npar;            /* Total number of parameters */
        public int nfree;           /* Number of free parameters */
        public int npegged;         /* Number of pegged parameters */
        public int nfunc;           /* Number of residuals (= num. of data points) */

        public double[] resid;       /* Final residuals
			          nfunc-vector, or 0 if not desired */
        public double[] xerror;      /* Final parameter uncertainties (1-sigma)
			          npar-vector, or 0 if not desired */
        public double[] covar;       /* Final parameter covariance matrix
			          npar x npar array, or 0 if not desired */
        public string version;    /* MPFIT version string */

        public MpResult(int numParameters)
        {
            xerror = new double[numParameters];
        }
    }
}