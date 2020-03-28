/* 
* MINPACK-1 Least Squares Fitting Library
*
* Original public domain version by B. Garbow, K. Hillstrom, J. More'
*   (Argonne National Laboratory, MINPACK project, March 1980)
* See the file DISCLAIMER for copyright information.
* 
* Tranlation to C Language by S. Moshier (moshier.net)
* Translation to C# Language by D. Cuccia (http://davidcuccia.wordpress.com)
* 
* Enhancements and packaging by C. Markwardt
*   (comparable to IDL fitting routine MPFIT
*    see http://cow.physics.wisc.edu/~craigm/idl/idl.html)
*/

/* Main MPFit library routines (double precision) 
   $Id: MPFit.cs,v 1.1 2010/05/04 dcuccia Exp $
 */

using System.Collections.Generic;

namespace Melville.CurveFit.MpFit
{
    public class MpConfig
    {
        public double ftol;    /* Relative chi-square convergence criterium */
        public double xtol;    /* Relative parameter convergence criterium */
        public double gtol;    /* Orthogonality convergence criterium */
        public double epsfcn;  /* Finite derivative step size */
        public double stepfactor; /* Initial step bound */
        public double covtol;  /* Range tolerance for covariance calculation */
        public int maxiter;    /* Maximum number of iterations.  If maxiter == 0,
                             then basic error checking is done, and parameter
                             errors/covariances are estimated based on input
                             parameter values, but no fitting iterations are done. */
        public int maxfev;     /* Maximum number of function evaluations */
        public int nprint;
        public int douserscale;/* Scale variables by user values?
		             1 = yes, user scale values in diag;
		             0 = no, variables scaled internally */
        public int nofinitecheck; /* Disable check for infinite quantities from user?
			        0 = do not perform check
			        1 = perform check 
		             */
        //mp
    }

    /// <summary>
    /// User-function delegate structure required by MPFit.Solve
    /// </summary>
    /// <param name="m">Number of functions (elemens of fvec)</param>
    /// <param name="npar">Number of variables (elements of x)</param>
    /// <param name="a">I - Parameters</param>
    /// <param name="fvec">O - function values</param>
    /// <param name="dvec">
    /// O - function derivatives (optional)
    /// "Array of ILists" to accomodate DelimitedArray IList implementation
    /// </param>
    /// <param name="prv">I/O - function private data (cast to object type in user function)</param>
    /// <returns></returns>
    public delegate int MpFunc(double[] a, double[] fvec, IList<double>[] dvec, object prv);

    /* Definition of a parameter constraint structure */

    /* Definition of results structure, for when fit completes */
}