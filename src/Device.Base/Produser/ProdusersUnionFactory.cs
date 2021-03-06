﻿using System;
using Autofac.Features.OwnedInstances;
using CSharpFunctionalExtensions;
using Domain.Device.Repository.Entities.ResponseProduser;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Infrastructure.Produser.KafkaProduser.Options;
using Infrastructure.Produser.WebClientProduser.Options;

namespace Domain.Device.Produser
{
    /// <summary>
    /// Фабрика по созданию объединения продюсеров.
    /// </summary>
    public class ProdusersUnionFactory<TIn>
    {
        private readonly Func<ProduserUnionOption, ProdusersUnion<TIn>> _produsersUnionFactory;
        private readonly Func<SignalRProduserOption, Owned<IProduser<SignalRProduserOption>>> _signalRFactory;
        private readonly Func<KafkaProduserOption, Owned<IProduser<KafkaProduserOption>>> _kafkaFactory;
        private readonly Func<WebClientProduserOption, Owned<IProduser<WebClientProduserOption>>> _webClientFactory;


        #region ctor
        public ProdusersUnionFactory(Func<ProduserUnionOption, ProdusersUnion<TIn>> produsersUnionFactory,
            Func<SignalRProduserOption, Owned<IProduser<SignalRProduserOption>>> signalRFactory,
            Func<KafkaProduserOption, Owned<IProduser<KafkaProduserOption>>> kafkaFactory,
            Func<WebClientProduserOption, Owned<IProduser<WebClientProduserOption>>> webClientFactory)
        {
            _produsersUnionFactory = produsersUnionFactory;
            _signalRFactory = signalRFactory;
            _kafkaFactory = kafkaFactory;
            _webClientFactory = webClientFactory;
        }
        #endregion


        #region Methode

        /// <summary>
        /// Добавляет созданные на базе опций продюссеры к ProdusersUnion
        /// </summary>
        /// <param name="unionOption"></param>
        public Result<ProdusersUnion<TIn>> CreateProduserUnion(ProduserUnionOption unionOption)
        {
            if (CheckEmptyStateAllProdussers(unionOption))
                return Result.Failure<ProdusersUnion<TIn>>("Все Коллекции продюссеров пусты");

            var produsersUnion = _produsersUnionFactory(unionOption);
            foreach (var option in unionOption.KafkaProduserOptions)
            {
                var prod = _kafkaFactory(option);
                produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
            foreach (var option in unionOption.SignalRProduserOptions)
            {
                var prod = _signalRFactory(option);
                produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }
            foreach (var option in unionOption.WebClientProduserOptions)
            {
                var prod = _webClientFactory(option);
                produsersUnion.AddProduser(option.Key, prod.Value, prod);
            }

            return Result.Ok(produsersUnion);
        }


        private static bool CheckEmptyStateAllProdussers(ProduserUnionOption unionOption)
        {
            return (unionOption.KafkaProduserOptions.Count == 0) && 
                   (unionOption.SignalRProduserOptions.Count == 0) &&
                   (unionOption.WebClientProduserOptions.Count == 0);

        }

        #endregion
    }
}