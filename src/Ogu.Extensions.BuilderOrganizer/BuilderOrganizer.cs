using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ogu.Extensions.BuilderOrganizer
{
    public class BuilderOrganizer<TBuilder>
    {
        private readonly Dictionary<double, (Func<TBuilder, object> FuncBuilder, string Tag)> _orderToBuilderDictionary = new Dictionary<double, (Func<TBuilder, object>, string)>();

        private readonly bool _ascending = true;

        public BuilderOrganizer()
        {
        }

        public BuilderOrganizer(bool ascending)
        {
            _ascending = ascending;
        }

        public BuilderOrganizer<TBuilder> AddOrUpdate(double order, Func<TBuilder, object> funcBuilder, string tag = null)
        {
            if (_orderToBuilderDictionary.TryGetValue(order, out var tuple))
            {
                _orderToBuilderDictionary.Remove(order);
                _orderToBuilderDictionary.Add(order, (funcBuilder, tag ?? tuple.Tag));
            }
            else
            {
                _orderToBuilderDictionary.Add(order, (funcBuilder, tag));
            }

            return this;
        }

        public void Build(TBuilder builder)
        {
            var orderedEnumerable = _ascending 
                ? _orderToBuilderDictionary.OrderBy(d => d.Key) 
                : _orderToBuilderDictionary.OrderByDescending(d => d.Key);

            foreach (var item in orderedEnumerable)
            {
                item.Value.FuncBuilder(builder);
            }
        }

        public BuilderOrganizer<TBuilder> Add(Func<TBuilder, object> func, string tag = null)
        {
            var newOrder = _orderToBuilderDictionary.Count > 0 ? _orderToBuilderDictionary.Keys.Max() + 1 : 1;

            _orderToBuilderDictionary.Add(newOrder, (func, tag));

            return this;
        }

        public BuilderOrganizer<TBuilder> Add(double order, Func<TBuilder, object> func, string tag = null, bool replaceIfOrderExists = false)
        {
            if (!_orderToBuilderDictionary.TryAdd(order, (func, tag)))
            {
                if (replaceIfOrderExists)
                {
                    var newOrder = _orderToBuilderDictionary.Keys.Max() + 1;
                    _orderToBuilderDictionary.Add(newOrder, (func, tag));
                    ReplaceIfExists(newOrder, order);
                }
                else
                {
                    throw new ArgumentException($"Couldn't be added due to existing order: {order} tag:{_orderToBuilderDictionary[order].Tag}", nameof(func));
                }
            }

            return this;
        }

        public BuilderOrganizer<TBuilder> Remove(double order)
        {
            _orderToBuilderDictionary.Remove(order);

            return this;
        }

        public BuilderOrganizer<TBuilder> ReplaceIfExists(double sourceOrder, double targetOrder)
        {
            if (!_orderToBuilderDictionary.TryGetValue(sourceOrder, out var sourceValue))
            {
                return this;
            }

            if (_orderToBuilderDictionary.TryGetValue(targetOrder, out var targetValue))
            {
                _orderToBuilderDictionary[targetOrder] = sourceValue;
                _orderToBuilderDictionary[sourceOrder] = targetValue;
            }
            else
            {
                _orderToBuilderDictionary.Add(targetOrder, (_orderToBuilderDictionary[sourceOrder]));
                _orderToBuilderDictionary.Remove(sourceOrder);
            }

            return this;
        }

        public static implicit operator string(BuilderOrganizer<TBuilder> builder)
        {
            return builder.ToString();
        }

        public override string ToString()
        {
            if (_orderToBuilderDictionary?.Count > 0)
            {
                var builder = new StringBuilder();

                var orderedEnumerable = _ascending
                    ? _orderToBuilderDictionary.OrderBy(d => d.Key)
                    : _orderToBuilderDictionary.OrderByDescending(d => d.Key);

                foreach (var orderAndTag in orderedEnumerable)
                {
                    builder.AppendFormat("Order: {0,7:#0.00} | Tag: {1}", orderAndTag.Key, orderAndTag.Value.Tag).AppendLine();
                }

                builder.Remove(builder.Length - 1, 1);

                return builder.ToString();
            }

            return string.Empty;
        }
    }
}