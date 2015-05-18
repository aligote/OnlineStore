﻿using System;
using System.ServiceModel;
using OnlineStore.ServiceContracts.ModelDTOs;

namespace OnlineStore.ServiceContracts
{
    [ServiceContract]
    public interface IOrderService
    {
        // 获得指定用户的购物车中商品数量
        [OperationContract]
        [FaultContract(typeof (FaultData))]
        int GetShoppingCartItemCount(Guid userId);

        // 将指定的商品添加到指定客户的购物车
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void AddProductToCart(Guid customerId, Guid productId, int quantity);

        // 根据指定的客户获取客户的购物车信息
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        ShoppingCartDto GetShoppingCart(Guid customerId);

        // 
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void UpdateShoppingCartItem(Guid shoppingCartItemId, int quantity);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteShoppingCartItem(Guid shoppingCartItemId);

        [OperationContract]
        [FaultContract(typeof(FaultData))]
        OrderDto Checkout(Guid customerId);
    }
}