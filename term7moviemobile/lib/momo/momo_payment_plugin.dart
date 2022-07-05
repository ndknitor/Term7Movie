import 'dart:async';
import 'dart:convert';

import 'package:flutter/services.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/momo/momo_payment_data.dart';
import 'package:term7moviemobile/momo/momo_payment_result.dart';

class MomoPaymentPlugin {
  static const MethodChannel _channel =
  const MethodChannel(Constants.channelName);

  Future<MomoPaymentResult> requestPayment(
      MomoPaymentData momoPaymentData) async {
    print(json.encode(momoPaymentData));
    try {
      Map<String, dynamic>? result =
      await _channel.invokeMapMethod(Constants.methodRequestPayment, momoPaymentData.toJson());
      return MomoPaymentResult.fromMap(result);
    } on PlatformException catch (error) {
      print(error);
      return MomoPaymentResult(
        isSuccess: false, token: '', phonenumber: '', status: 0,
      );
    }
  }
}
