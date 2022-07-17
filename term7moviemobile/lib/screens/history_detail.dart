import 'package:barcode_widget/barcode_widget.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/history_detail_controller.dart';
import 'package:term7moviemobile/utils/theme.dart';

class HistoryDetailScreen extends StatefulWidget {
  const HistoryDetailScreen({Key? key}) : super(key: key);

  @override
  State<HistoryDetailScreen> createState() => _HistoryDetailScreenState();
}

class _HistoryDetailScreenState extends State<HistoryDetailScreen> {
  late HistoryDetailController controller;

  @override
  void initState() {
    Get.delete<HistoryDetailController>();
    controller = Get.put(HistoryDetailController());
    controller.fetchTransactionDetail();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    return Scaffold(
        body: Obx(() => LoadingOverlay(
              isLoading: controller.isLoading.value,
              color: MyTheme.backgroundColor,
              progressIndicator: const CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
              ),
              opacity: 1,
              child: controller.transaction != null ? Stack(
                children: [
                  Positioned(
                    child: Column(
                      children: [
                        Expanded(
                          flex: 8,
                          child: Container(color: MyTheme.primaryColor),
                        ),
                        Expanded(
                          flex: 4,
                          child: Container(
                            color: MyTheme.borderColor,
                          ),
                        )
                      ],
                    ),
                  ),
                  Positioned(
                    left: 16,
                    right: 16,
                    top: 32,
                    bottom: 16,
                    child: Column(
                      children: [
                        Expanded(
                          child: Padding(
                            padding: const EdgeInsets.symmetric(vertical: 24),
                            child: Container(
                              decoration: BoxDecoration(
                                color: Colors.white,
                                borderRadius: BorderRadius.circular(8),
                              ),
                              padding: EdgeInsets.symmetric(
                                  horizontal: 24, vertical: 16),
                              child: Column(
                                children: [
                                  Row(
                                    mainAxisAlignment: MainAxisAlignment.start,
                                    crossAxisAlignment:
                                    CrossAxisAlignment.start,
                                    children: [
                                      ClipRRect(
                                        borderRadius: BorderRadius.circular(10),
                                        child: Image.network(
                                          controller
                                              .transaction!
                                              .showtime!
                                              .movie!
                                              .posterImgUrl!
                                              .length ==
                                              0
                                              ? 'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png'
                                              : controller
                                              .transaction!
                                              .showtime!
                                              .movie!
                                              .posterImgUrl!,
                                          height: 110,
                                          width: 80,
                                          fit: BoxFit.cover,
                                        ),
                                      ),
                                      const SizedBox(
                                        width: 8,
                                      ),
                                      Column(
                                        crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                        children: [
                                          Container(
                                            width: size.width - 168,
                                            margin: EdgeInsets.only(bottom: 4),
                                            child: Text(
                                              controller.transaction!.showtime!
                                                  .movie!.title!,
                                              softWrap: true,
                                              style: TextStyle(
                                                  fontSize: 18,
                                                  color: MyTheme.textColor,
                                                  fontWeight: FontWeight.w700),
                                            ),
                                          ),
                                          Text(
                                            controller.transaction!.showtime!
                                                .movie!.duration
                                                .toString() +
                                                ' min',
                                            style: TextStyle(
                                              fontSize: 14,
                                              color: MyTheme.grayColor,
                                            ),
                                          ),
                                          // Row(
                                          //   mainAxisSize: MainAxisSize.min,
                                          //   children: controller
                                          //       .movie!.categories!
                                          //       .map((e) =>
                                          //       Builder(builder: (context) {
                                          //         return Container(
                                          //           margin: EdgeInsets.only(
                                          //               right: 8),
                                          //           alignment:
                                          //           Alignment.center,
                                          //           decoration: BoxDecoration(
                                          //             borderRadius:
                                          //             BorderRadius
                                          //                 .circular(4),
                                          //             color: HexColor(
                                          //                 e.color!)
                                          //                 .withOpacity(0.2),
                                          //           ),
                                          //           padding:
                                          //           EdgeInsets.symmetric(
                                          //               vertical: 2,
                                          //               horizontal: 6),
                                          //           child: Text(
                                          //             e.name!,
                                          //             style: TextStyle(
                                          //               fontWeight:
                                          //               FontWeight.w600,
                                          //               fontSize: 10,
                                          //               color: HexColor(
                                          //                   e.color!),
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
                                      )
                                    ],
                                  ),
                                  SizedBox(
                                    height: 40,
                                  ),
                                  Text("Transaction ID"),
                                  Padding(
                                    padding:
                                    const EdgeInsets.symmetric(vertical: 8),
                                    child: Center(
                                      child: Text(
                                        controller.transaction!.transactionId!,
                                        style: TextStyle(
                                          fontWeight: FontWeight.bold,
                                          fontSize: 12,
                                        ),
                                      ),
                                    ),
                                  ),
                                  Divider(
                                    color: Colors.grey,
                                  ),
                                  Padding(
                                    padding:
                                    const EdgeInsets.symmetric(vertical: 8),
                                    child: Row(
                                      mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                      children: [
                                        Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              DateFormat.Hm().format(
                                                  DateTime.parse(controller
                                                      .transaction!
                                                      .showtime!
                                                      .startTime!)
                                                      .add(Duration(hours: 7))),
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                            SizedBox(
                                              height: 8,
                                            ),
                                            Text("Time")
                                          ],
                                        ),
                                        Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.end,
                                          children: [
                                            Text(
                                              DateFormat.yMMMd()
                                                  .format(DateTime.parse(
                                                  controller
                                                      .transaction!
                                                      .showtime!
                                                      .startTime!))
                                                  .toString(),
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                            SizedBox(
                                              height: 8,
                                            ),
                                            Text("Date")
                                          ],
                                        )
                                      ],
                                    ),
                                  ),
                                  Padding(
                                    padding:
                                    const EdgeInsets.symmetric(vertical: 8),
                                    child: Row(
                                      mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                      children: [
                                        Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              controller.transaction!.showtime!
                                                  .room!.no
                                                  .toString(),
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                            SizedBox(
                                              height: 8,
                                            ),
                                            Text("Room No")
                                          ],
                                        ),
                                        Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.end,
                                          children: [
                                            Text(
                                              controller.transaction!.showtime!
                                                  .theaterName!,
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                            SizedBox(
                                              height: 8,
                                            ),
                                            Text("Cinema")
                                          ],
                                        )
                                      ],
                                    ),
                                  ),
                                  Padding(
                                    padding:
                                    const EdgeInsets.symmetric(vertical: 8),
                                    child: Row(
                                      mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                      children: [
                                        Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              controller.transaction!.tickets!
                                                  .map((e) => e.seat!.name)
                                                  .toList()
                                                  .join(","),
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                            SizedBox(
                                              height: 8,
                                            ),
                                            Text("Seat Number")
                                          ],
                                        ),
                                        Column(
                                          crossAxisAlignment:
                                          CrossAxisAlignment.end,
                                          children: [
                                            Text(
                                              controller.transaction!.total
                                                  .toString() +
                                                  " VND",
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                            SizedBox(
                                              height: 8,
                                            ),
                                            Text("Price")
                                          ],
                                        )
                                      ],
                                    ),
                                  ),
                                  Divider(
                                    color: Colors.grey,
                                  ),
                                  Padding(
                                    padding:
                                    const EdgeInsets.symmetric(vertical: 8),
                                    child: Text(
                                      "Your E-Ticket",
                                      style: TextStyle(
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                  ),
                                  Expanded(
                                    child: FutureBuilder<String?>(
                                      future: generateBarcode(
                                          controller.transaction!.qrCodeUrl!),
                                      builder: (context, snapshot) {
                                        if (snapshot.hasData) {
                                          return SvgPicture.string(
                                              snapshot.data ?? "");
                                        }
                                        return Center(
                                          child: CircularProgressIndicator(
                                            valueColor: AlwaysStoppedAnimation(
                                                MyTheme.primaryColor),
                                          ),
                                        );
                                      },
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                        GestureDetector(
                          onTap: () {
                            Get.toNamed("/history");
                          },
                          child: Container(
                            decoration: BoxDecoration(
                              color: MyTheme.primaryColor,
                              borderRadius: BorderRadius.circular(8),
                            ),
                            padding: EdgeInsets.symmetric(vertical: 16),
                            child: Center(
                              child: Text(
                                "Close",
                                style: TextStyle(
                                  fontWeight: FontWeight.bold,
                                  color: Colors.white,
                                ),
                              ),
                            ),
                          ),
                        )
                      ],
                    ),
                  ),
                  Positioned(
                    left: 0,
                    right: 0,
                    top: 190,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      crossAxisAlignment: CrossAxisAlignment.center,
                      children: [
                        CircleAvatar(
                          radius: 16,
                          backgroundColor: MyTheme.primaryColor,
                        ),
                        Expanded(
                          child: Text(
                            "--------------------------------------",
                            style: TextStyle(color: MyTheme.borderColor),
                          ),
                        ),
                        CircleAvatar(
                          radius: 16,
                          backgroundColor: MyTheme.primaryColor,
                        )
                      ],
                    ),
                  ),
                ],
              ) : Container(),
            )));
  }
}

Future<String?> generateBarcode(String barcode) async {
  final dm = Barcode.code128();
  return dm.toSvg(barcode, width: 500, height: 100, drawText: false);
}
