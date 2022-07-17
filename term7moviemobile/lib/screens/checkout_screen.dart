import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:momo_vn/momo_vn.dart';
import 'package:term7moviemobile/controllers/checkout_controller.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/transaction_services.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/convert_color.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';
import 'package:uuid/uuid.dart';

class CheckOutScreen extends StatefulWidget {
  const CheckOutScreen({Key? key}) : super(key: key);

  @override
  State<CheckOutScreen> createState() => _CheckOutScreenState();
}

class _CheckOutScreenState extends State<CheckOutScreen> {
  late CheckOutController controller;
  late MomoVn _momoPay;
  late PaymentResponse _momoPaymentResult;
  late String _paymentStatus;

  _setState() {
    _paymentStatus = 'Đã chuyển thanh toán';
    if (_momoPaymentResult.isSuccess == true) {
      _paymentStatus += "\nTình trạng: Thành công.";
      // _paymentStatus +=
      //     "\nSố điện thoại: " + _momoPaymentResult.phoneNumber.toString();
      // _paymentStatus += "\nExtra: " + _momoPaymentResult.extra!;
      // _paymentStatus += "\nToken: " + _momoPaymentResult.token.toString();
    } else {
      _paymentStatus += "\nTình trạng: Thất bại.";
      // _paymentStatus += "\nExtra: " + _momoPaymentResult.extra.toString();
      // _paymentStatus += "\nMã lỗi: " + _momoPaymentResult.status.toString();
    }
  }

  _handlePaymentSuccess(PaymentResponse response) {
    _momoPaymentResult = response;
    _setState();
    controller.getSuccessSnackBar();
  }

  _handlePaymentError(PaymentResponse response) {
    _momoPaymentResult = response;
    _setState();
   controller.getErrorSnackBar();
  }

  @override
  void initState() {
    super.initState();
    // Get.delete<CheckOutController>();
    controller = Get.put(CheckOutController());
    _momoPay = MomoVn();
    _momoPay.on(MomoVn.EVENT_PAYMENT_SUCCESS, _handlePaymentSuccess);
    _momoPay.on(MomoVn.EVENT_PAYMENT_ERROR, _handlePaymentError);
    _paymentStatus = "";
  }

  @override
  void dispose() {
    super.dispose();
    _momoPay.clear();
  }

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    return Scaffold(
        body: SingleChildScrollView(
      child: SafeArea(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                ArrowBack(color: MyTheme.bottomBarColor),
                Padding(
                  padding: const EdgeInsets.only(top: 64.0, right: 24.0),
                  child: Text(
                    "05:00",
                    style: TextStyle(
                        fontWeight: FontWeight.w600,
                        color: MyTheme.warningColor),
                  ),
                ),
              ],
            ),
            Container(
              margin: const EdgeInsets.symmetric(horizontal: 24),
              padding: const EdgeInsets.symmetric(vertical: 24),
              decoration: const BoxDecoration(
                border: Border(
                    bottom: BorderSide(color: MyTheme.borderColor, width: 1)),
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Container(
                    width: size.width / 4,
                    alignment: Alignment.centerLeft,
                    child: ClipRRect(
                      child: Image.network(
                       'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png',
                      ),
                      borderRadius: BorderRadius.circular(8),
                    ),
                  ),
                  MovieInfoWidget(
                      size: size, movie: controller.showtime!.movie! )
                ],
              ),
            ),
            buildPriceTag('Cinema', controller.showtime?.theaterName ?? ''),
            buildPriceTag('Room No', controller.showtime?.room?.no.toString() ?? ''),
            buildPriceTag(
                'Date & Time',
                DateFormat.yMMMd()
                    .format(DateTime.parse(
                    controller.showtime!.startTime!.split('T')[0]))
                    .toString()
                 +
                    "   " +
                    DateFormat.Hm().format(
                        DateTime.parse(controller.showtime!.startTime!)
                            .add(Duration(hours: 7)))),
            buildPriceTag('Seat Number',
                controller.tickets!.map((e) => e.seat!.name)
                    .toList()
                    .join(", ")),
            buildPriceTag('Total', controller.total.toStringAsFixed(0) + " VND"),
            Container(
              margin: const EdgeInsets.symmetric(horizontal: 24),
              padding: const EdgeInsets.only(bottom: 24),
              decoration: const BoxDecoration(
                  border: Border(
                      bottom:
                          BorderSide(color: MyTheme.borderColor, width: 1))),
            ),
            SizedBox(
              height: 24,
            ),
            Row(
              children: [
                Expanded(
                  child: Center(
                    child: GestureDetector(
                      onTap: () async {
                        // controller.createPaymentSuccessNotification();
                        if (!controller.isLoading.value) {
                          controller.isLoading.value = true;
                          MomoPaymentInfo options = MomoPaymentInfo(
                              merchantName: "F-cine",
                              appScheme: "MOxx",
                              merchantCode: 'MOMOSIRF20220103',
                              partnerCode: 'MOMOSIRF20220103',
                              amount: controller.total.toInt(),
                              orderId: '12321312',
                              orderLabel: 'vé xem phim',
                              merchantNameLabel: "HLGD",
                              fee: 0,
                              description: 'Thanh toán vé xem phim',
                              username: 'Fcinema',
                              partner: 'merchant',
                              extra: "{\"key1\":\"value1\",\"key2\":\"value2\"}",
                              isTestMode: true);
                          try {
                            var res = await TransactionServices.postTransaction({
                              'transactionId': controller.transactionId,
                              'showtimeId': controller.showtime!.id,
                              'ticketIdList': controller.tickets!.map((e) => e.id).toList(),
                            });
                            print(res);
                            if (res.statusCode == 200) {
                              _momoPay.open(options);
                            } else {
                              TransactionServices.getTransactionById(controller.transactionId).then((value) => _momoPay.open(options));
                            }
                            // if (controller.transactionId == '') {
                            //   controller.transactionId = Uuid().v4().toString();
                            //
                            // } else {
                            //   TransactionServices.getTransactionById(controller.transactionId).then((value) => _momoPay.open(options));
                            // }
                          } catch (e) {
                            debugPrint(e.toString());
                          } finally {
                            controller.isLoading.value = false;
                          }
                        }
                      },
                      child: Container(
                        height: size.height / 16,
                        width: size.width / 2,
                        decoration: BoxDecoration(
                            color:  controller.isLoading.value ? MyTheme.primaryColor.withOpacity(0.6) : MyTheme.primaryColor,
                            borderRadius: BorderRadius.circular(20)),
                        alignment: Alignment.center,
                        child: const Text(
                          'Pay',
                          style: TextStyle(
                              fontWeight: FontWeight.w500, color: Colors.white),
                        ),
                      ),
                    ),
                  ),
                ),
              ],
            )
          ],
        ),
      ),
    ));
  }

  Container buildPriceTag(String content, String price) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 24),
      padding: const EdgeInsets.symmetric(vertical: 12),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(
            content,
          ),
          Text(
            price,
            style: TextStyle(fontWeight: FontWeight.w500),
          ),
        ],
      ),
    );
  }
}

class MovieInfoWidget extends StatelessWidget {
  const MovieInfoWidget({
    Key? key,
    required this.size,
    required this.movie,
  }) : super(key: key);

  final Size size;
  final MovieModel movie;

  @override
  Widget build(BuildContext context) {
    return Expanded(
        child: Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      mainAxisAlignment: MainAxisAlignment.start,
      children: [
        Container(
          padding: const EdgeInsets.only(left: 8, bottom: 8),
          width: size.width,
          child: Text(
            movie.title ?? '',
            style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
          ),
        ),
        Container(
          padding: const EdgeInsets.only(left: 8, bottom: 8),
          width: size.width,
          child: Text(
            movie.duration.toString() + ' min',
          ),
        ),
        // Row(
        //   children: movie.categories!.map((e) =>
        //       Builder(builder: (context) {
        //         return Container(
        //           margin: EdgeInsets.only(
        //               right: 8),
        //           alignment:
        //           Alignment.center,
        //           decoration:
        //           BoxDecoration(
        //             borderRadius:
        //             BorderRadius
        //                 .circular(4),
        //             color: HexColor(
        //                 e.color!)
        //                 .withOpacity(0.2),
        //           ),
        //           padding: EdgeInsets
        //               .symmetric(
        //               vertical: 2,
        //               horizontal: 6),
        //           child: Text(
        //             e.name!,
        //             style: TextStyle(
        //               fontWeight:
        //               FontWeight.w600,
        //               fontSize: 10,
        //               color: HexColor(e.color!),
        //               decoration:
        //               TextDecoration
        //                   .none,
        //             ),
        //           ),
        //         );
        //       }))
        //       .toList(),
        // ),
      ],
    ));
  }
}
