using Smrf.AppLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class DeltaQMaxHeap : BinaryHeap<Community, Single>
    {
        //*************************************************************************
        //  Constructor: DeltaQMaxHeap()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaQMaxHeap" /> class
        /// with a specified initial capacity.
        /// </summary>
        ///
        /// <param name="initialCapacity">
        /// Initial capacity.  Must be non-negative.
        /// </param>
        //*************************************************************************

        public DeltaQMaxHeap
        (
            Int32 initialCapacity
        )
            : base(initialCapacity, new DeltaQComparer())
        {
            // (Do nothing else.)

            AssertValid();
        }


        //*************************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*************************************************************************

        // [Conditional("DEBUG")]

        public override void
        AssertValid()
        {
            base.AssertValid();

            // (Do nothing else.)
        }


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // (None.)
    }


    //*****************************************************************************
    //  Class: DeltaQComparer
    //
    /// <summary>
    /// Compares two delta Q values.
    /// </summary>
    //*****************************************************************************

    public class DeltaQComparer : IComparer<Single>
    {
        //*************************************************************************
        //  Constructor: DeltaQComparer()
        //
        /// <summary>
        /// Initializes a new instance of the <see cref="DeltaQComparer" /> class.
        /// </summary>
        //*************************************************************************

        public DeltaQComparer()
        {
            // (Do nothing.)

        }

        //*************************************************************************
        //  Method: Compare()
        //
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less
        /// than, equal to, or greater than the other.
        /// </summary>
        ///
        /// <param name="x">
        /// The first object to compare.
        /// </param>
        ///
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        ///
        /// <returns>
        /// See the interface definition.
        /// </returns>
        //*************************************************************************

        public Int32
        Compare
        (
            Single x,
            Single y
        )
        {

            return (x.CompareTo(y));
        }


    }
}
