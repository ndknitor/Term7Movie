import 'package:awesome_notifications/awesome_notifications.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/services/transaction_services.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:uuid/uuid.dart';

class CheckOutController extends GetxController {
  var arguments = Get.arguments;
  ShowtimeModel? showtime;
  List<TicketModel>? tickets;
  double total = 0;
  String transactionId = Uuid().v4().toString();
  var isSuccess = false.obs;
  var isLoading = false.obs;

  @override
  void onInit() {
    super.onInit();
    showtime = arguments['showtime'];
    tickets = arguments['tickets'];
    total = arguments['total'];
  }

  getErrorSnackBar() {
    Get.snackbar(
      'Error',
      'Payment Failed',
      snackPosition: SnackPosition.BOTTOM,
      backgroundColor: MyTheme.errorColor,
      colorText: Colors.white,
      borderRadius: 10,
      margin: const EdgeInsets.only(bottom: 10, left: 10, right: 10),
    );
  }

  getSuccessSnackBar() async {
    var res = await TransactionServices.postCompletePayment(transactionId);
    if (res.statusCode == 200) {
      Get.toNamed("/history/$transactionId");
      Get.snackbar(
        "Success",
        'Payment Successfully',
        snackPosition: SnackPosition.BOTTOM,
        backgroundColor: MyTheme.successColor,
        colorText: Colors.white,
        borderRadius: 10,
        margin: const EdgeInsets.only(bottom: 10, left: 10, right: 10),
      );
      createPaymentSuccessNotification();
    } else {
      getErrorSnackBar();
    }
  }

  Future<void> createPaymentSuccessNotification() async {
    await AwesomeNotifications().createNotification(
      content: NotificationContent(
        id: 1,
        channelKey: 'basic_channel',
        title:
        '${Emojis.money_money_bag + Emojis.activites_ticket} Payment Success!!!',
        body: 'Your payment is success scan your barcode and enjoy your movie',
        bigPicture: 'asset://assets/images/map.png',
        showWhen: true,
        displayOnBackground: true,
        notificationLayout: NotificationLayout.BigPicture,
      ),
    ).catchError((err) => print(err));
  }
}
