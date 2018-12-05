using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsCSharp;
using EventPropsClasses;

// *** I had to change this
using ProductDB = EventDBClasses.ProductSQLDB;

// *** I added this
using System.Data;

namespace EventClasses
{
    public class Product : BaseBusiness
    {
        #region properties
        public int ID
        {
            get
            {
                return ((ProductProps)mProps).ID;
            }
        }

        public string Description
        {
            get
            {
                return ((ProductProps)mProps).description;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).description))
                {
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        mRules.RuleBroken("Description", false);
                        ((ProductProps)mProps).description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Description must be between 1 and 50 characters");
                    }
                }
            }
        }

        public string Code
        {
            get
            {
                return ((ProductProps)mProps).code;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).code))
                {
                    if (value.Length >= 1 && value.Length <= 10)
                    {
                        mRules.RuleBroken("ProductCode", false);
                        ((ProductProps)mProps).code = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Code must be between 1 and 10 characters");
                    }
                }
            }
        }

        public decimal Price
        {
            get
            {
                return ((ProductProps)mProps).price;
            }

            set
            {
                if (!(value == ((ProductProps)mProps).price))
                {
                    if (value > 0)
                    {
                        mRules.RuleBroken("UnitPrice", false);
                        ((ProductProps)mProps).price = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Price must be a positive number");
                    }
                }
            }
        }

        public int Quantity
        {
            get
            {
                return ((ProductProps)mProps).quantity;
            }
            
            set
            {
                if (!(value == ((ProductProps)mProps).quantity))
                {
                    if (value >= 0)
                    {
                        ((ProductProps)mProps).quantity = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Quantity must be a positive number");
                    }
                }
            }
            
        }
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Product() : base()
        {
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Product(string cnString)
            : base(cnString)
        {
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Product(int key, string cnString)
            : base(key, cnString)
        {
        }

        public Product(int key)
            : base(key)
        {
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Product(ProductProps props)
            : base(props)
        {
        }

        public Product(ProductProps props, string cnString)
            : base(props, cnString)
        {
        }
        #endregion

        #region methods
        public override object GetList()
        {
            throw new NotImplementedException();
        }

        protected override void SetDefaultProperties()
        {
            //throw new NotImplementedException();
        }

        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("ProductCode", true);
            mRules.RuleBroken("Description", true);
            mRules.RuleBroken("UnitPrice", true);
            //mRules.RuleBroken("OnHandQuantity", true);
        }

        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new ProductDB();
                mdbWriteable = new ProductDB();
            }

            else
            {
                mdbReadable = new ProductDB(this.mConnectionString);
                mdbWriteable = new ProductDB(this.mConnectionString);
            }
        }
        #endregion
    }
}
