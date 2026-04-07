using System;
using System.Globalization;

namespace Tables
{
    public partial class Constant
    {
        public static int GetInt(string key)
        {
            var constant = GetRequired(key);
            return constant.AsInt();
        }

        public static float GetFloat(string key)
        {
            var constant = GetRequired(key);
            return constant.AsFloat();
        }

        public static long GetLong(string key)
        {
            var constant = GetRequired(key);
            return constant.AsLong();
        }

        public static string GetString(string key)
        {
            var constant = GetRequired(key);
            return constant.AsString();
        }

        bool isParsed;
        int intValue;
        float floatValue;
        long longValue;
        string stringValue;

        static Constant GetRequired(string key)
        {
            var constant = Get(key);
            if (constant == null)
            {
                throw new InvalidOperationException($"Constant not found. key={key}");
            }

            return constant;
        }

        void EnsureParsed()
        {
            if (isParsed)
            {
                return;
            }

            switch (dataType)
            {
                case DataType.Int:
                    intValue = int.Parse(rawValue, CultureInfo.InvariantCulture);
                    break;

                case DataType.Float:
                    floatValue = float.Parse(rawValue, CultureInfo.InvariantCulture);
                    break;

                case DataType.Long:
                    longValue = long.Parse(rawValue, CultureInfo.InvariantCulture);
                    break;

                case DataType.String:
                    stringValue = rawValue;
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported Constant dataType. key={Key}, dataType={dataType}");
            }

            isParsed = true;
        }

        public int AsInt()
        {
            EnsureParsed();

            if (dataType != DataType.Int)
            {
                throw new InvalidOperationException($"Constant type mismatch. key={Key}, actual={dataType}, expected={DataType.Int}");
            }

            return intValue;
        }

        public float AsFloat()
        {
            EnsureParsed();

            if (dataType != DataType.Float)
            {
                throw new InvalidOperationException($"Constant type mismatch. key={Key}, actual={dataType}, expected={DataType.Float}");
            }

            return floatValue;
        }

        public long AsLong()
        {
            EnsureParsed();

            if (dataType != DataType.Long)
            {
                throw new InvalidOperationException($"Constant type mismatch. key={Key}, actual={dataType}, expected={DataType.Long}");
            }

            return longValue;
        }

        public string AsString()
        {
            EnsureParsed();

            if (dataType != DataType.String)
            {
                throw new InvalidOperationException($"Constant type mismatch. key={Key}, actual={dataType}, expected={DataType.String}");
            }

            return stringValue;
        }

        public bool TryGetInt(out int value)
        {
            value = default;
            if (dataType != DataType.Int)
            {
                return false;
            }

            EnsureParsed();
            value = intValue;
            return true;
        }

        public bool TryGetFloat(out float value)
        {
            value = default;
            if (dataType != DataType.Float)
            {
                return false;
            }

            EnsureParsed();
            value = floatValue;
            return true;
        }

        public bool TryGetLong(out long value)
        {
            value = default;
            if (dataType != DataType.Long)
            {
                return false;
            }

            EnsureParsed();
            value = longValue;
            return true;
        }

        public bool TryGetString(out string value)
        {
            value = default;
            if (dataType != DataType.String)
            {
                return false;
            }

            EnsureParsed();
            value = stringValue;
            return true;
        }
    }
}
