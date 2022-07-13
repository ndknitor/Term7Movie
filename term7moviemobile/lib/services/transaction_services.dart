import 'dart:convert';
import 'package:dio/dio.dart';
import 'package:term7moviemobile/models/transaction_model.dart';
import 'package:term7moviemobile/services/api.dart';

class TransactionServices {
  static Future<Response> postTransaction(Map<String, dynamic>? queryParams) async {
    print(queryParams);
    var res = await Api().api.post('/transactions', data: jsonEncode(queryParams));
    // print(res);
    return res;
  }

  static Future<Response> postCompletePayment(String transactionId) async {
    var res = await Api().api.get('/transactions/complete-payment?transactionId=$transactionId');
    // print(res);
    return res;
  }

  static Future<List<TransactionModel>> getTransactions(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/transactions', queryParameters: queryParams);
    return List<TransactionModel>.from(res.data['result']['results'].map((x) => TransactionModel.fromJson(x)));
  }

  static Future<TransactionModel> getTransactionById(String transactionId) async {
    var res = await Api().api.get('/transactions/$transactionId');
    return TransactionModel.fromJson(res.data['result']);
  }
}