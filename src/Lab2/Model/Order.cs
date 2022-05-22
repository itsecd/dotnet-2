using System;
using System.Collections.Generic;
using System.Linq;
using static Lab2.Model.OrderStatus;

namespace Lab2.Model
{
    /// <summary>
    /// класс заказа
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Список продуктов
        /// </summary>
        public List<Product>? Products { get; set; }
        /// <summary>
        /// Идентификатор покупатели
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Время заказа
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Статус заказа
        /// </summary>
        public Status OrderStatus { get; set; }
        /// <summary>
        /// Сумма цены заказа
        /// </summary>
        public float SumOrder => Products!.Sum(product => product.Price);
    }
}
