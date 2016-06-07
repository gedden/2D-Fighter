using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Owl
{
    public class OwlUtil
    {
        /// <summary>
        /// Useful for iterating over enums
        /// 
        /// ie: foreach( var faction in OwlUtil.GetValues<Faction>() )
        /// </summary>
        /// <returns>The values.</returns>
        /// <typeparam name="T">The enum to iterate over</typeparam>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Destroy all the children of the passed in transform
        /// </summary>
        /// <param name="parent"></param>
        public static void DestroyChildren(Transform parent)
        {
            foreach (Transform child in parent)
                UnityEngine.Object.Destroy(child.gameObject);
        }

        /// <summary>
        /// Destroy all the children of the passed in transform
        /// which contain a component of type T
        /// </summary>
        /// <param name="parent"></param>
        public static void DestroyChildren<T>(Transform parent) where T : Component
        {
            foreach (Transform child in parent)
            {
                var sub = child.GetComponent<T>();

                if (sub != null)
                    UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public static void DestroyImmediateChildren(Transform parent)
        {
            var children = parent.Cast<Transform>().ToList();
            foreach (Transform child in children)
                UnityEngine.Object.DestroyImmediate(child.gameObject);
        }

        /// <summary>
        /// Anchorses to corners.
        /// </summary>
        /// <param name="t">T.</param>
        public static void AnchorsToCorners(RectTransform t)
        {
            RectTransform pt = t.parent as RectTransform;

            // Sanity check
            if (t == null || pt == null) return;

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width, t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width, t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }

        /// <summary>
        /// Run the file on the file system with the passed in argument
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="arguments">Arguments.</param>
        public static int Run(string path, string arguments = null)
        {
            try
            {
                path = System.IO.Path.GetFullPath(path);

                Process proc = new Process();
                proc.StartInfo.FileName = path;

                if (arguments != null)
                    proc.StartInfo.Arguments = arguments;

                proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                proc.Start();

                proc.WaitForExit();
                return proc.ExitCode;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
            return -1;
        }

        /// <summary>
        /// Check if a paramter exists with an animator.
        /// </summary>
        /// <param name="_animator">Animator.</param>
        /// <param name="parameterName">Parameter Name.</param>
        public static bool ContainsParamater(Animator _animator, string parameterName)
        {
            foreach (AnimatorControllerParameter paramater in _animator.parameters)
            {
                if (paramater.name == parameterName) return true;
            }
            return false;
        }

        /// <summary>
        /// Filter a list by a property and value to check if they are EQUAL.
        /// </summary>
        /// <param name="collection">List to filter.</param>
        /// <param name="property">Property to filter by.</param>
        /// <param name="filterValue">Value to check if equal to.</param>
        public static List<T> FilterList<T, U>(List<T> collection, string property, U filterValue, ExpressionType expression = ExpressionType.Equal) where U : IComparable
        {
            var filteredCollection = new List<T>();
            try
            {
                foreach (var item in collection)
                {
                    var uncastedValue = FindPropertyIfExists(item, property);
                    if (uncastedValue == null) continue;
                    if (!(uncastedValue is U)) throw new InvalidCastException();
                    U propertyValue = (U)uncastedValue;

                    if (expression == ExpressionType.Equal && propertyValue.Equals(filterValue))
                    {
                        filteredCollection.Add(item);
                    }
                    else if (expression == ExpressionType.GreaterThan && IsGreaterThan(propertyValue, filterValue))
                    {
                        filteredCollection.Add(item);
                    }
                    else if (expression == ExpressionType.LessThan && IsLessThan(propertyValue, filterValue))
                    {
                        filteredCollection.Add(item);
                    }
                }
            }
            catch (InvalidCastException e)
            {
                UnityEngine.Debug.Log("Cannot convert " + property + " to " + filterValue.GetType());
                UnityEngine.Debug.Log(e);
            }
            return filteredCollection.Cast<T>().ToList();
        }

        /// <summary>
        /// Determine if an object satisfies a property and a comparison.
        /// </summary>
        /// <param name="item">Object being checked.</param>
        /// <param name="property">Property to compare.</param>
        /// <param name="filterValue">Value to check against.</param>
        /// /// <param name="expression">The expression type (equals, lessThan, greaterThan, etc).</param>
        public static bool FilterByPropertyComparison<T, U>(T item, string property, U filterValue, ExpressionType expression = ExpressionType.Equal) where U : IComparable
        {
            var uncastedValue = FindPropertyIfExists(item, property);
            if (!(uncastedValue is U)) throw new InvalidCastException();
            U propertyValue = (U)uncastedValue;
            if (propertyValue.GetType() != filterValue.GetType())
            {
                UnityEngine.Debug.Log("The property Value and the filter value are not of the same type! \n"
                                      + "Property " + property.ToString() + " Filter Value: " + filterValue.ToString());
            }

            return CheckComparison(propertyValue, filterValue, expression);
        }

        /// <summary>
        /// Uses reflection to find a property of a class.
        /// </summary>
        /// <param name="item">The class you are accessing a property on.</param>
        /// <param name="property">Property to access.</param>
        public static object FindPropertyIfExists<T>(T item, string property)
        {
            try
            {
                var propertyInfo = item.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null) throw new NotSupportedException("Property: " + property + " given does not exist for item: " + item.ToString());

                var propertyValue = propertyInfo.GetValue(item, null);
                return propertyValue;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
            return null;
        }

        /// <summary>
        /// Filter a list by a type.
        /// </summary>
        /// <param name="collection">List to filter.</param>
        /// 
        public static List<object> FilterByType<T>(List<object> collection)
        {
            return collection.Where(o => o is T).ToList();
        }

        /// <summary>
        /// An alternative to nameof, which is in c# 6.0 but not officially supported yet.
        /// </summary>
        /// <param name="method">Method you want the name of.</param>
        public static string GetFunctionName(Action method)
        {
            return method.Method.Name;
        }

        /// <summary>
        /// An alternative to nameof, which is in c# 6.0 but not officially supported yet.
        /// </summary>
        /// <param name="method">Method you want the name of.</param>
        public static string GetFunctionName<T>(Action<T> method)
        {
            return method.Method.Name;
        }

        /// <summary>
        /// An alternative to nameof, which is in c# 6.0 but not officially supported yet.
        /// </summary>
        /// <param name="property">Property you want the name of.</param>
        public static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            return ((MemberExpression)property.Body).Member.Name;
        }

        #region Comparable Operators
        public static bool IsGreaterThan<T>(T value, T other) where T : IComparable
        {
            return value.CompareTo(other) > 0;
        }

        public static bool IsLessThan<T>(T value, T other) where T : IComparable
        {
            return value.CompareTo(other) < 0;
        }

        private static bool CheckComparison(IComparable item, IComparable other, ExpressionType expression)
        {
            switch (expression)
            {
                case ExpressionType.Equal:
                    return item.Equals(other);
                case ExpressionType.GreaterThan:
                    return IsGreaterThan(item, other);
                case ExpressionType.LessThan:
                    return IsLessThan(item, other);
                default:
                    UnityEngine.Debug.Log("Unsupported expression type!");
                    return false;
            }
        }
        #endregion

        /// <summary>
        /// Takes a resolution in string form and converts to Vector2.
        /// </summary>
        /// <param name="resString">Resolution string to convert.</param>
        public static Vector2 ConvertResolution(string resString)
        {
            Vector2 resolution = new Vector2();
            string[] numbers = resString.Split('x');
            string firstNumber = numbers[0].TrimEnd();
            string secondNumber = numbers[1].TrimStart();

            int widthRes;
            int heightRes;
            int.TryParse(firstNumber, out widthRes);
            int.TryParse(secondNumber, out heightRes);
            resolution.x = widthRes;
            resolution.y = heightRes;
            return resolution;
        }
    }
}

