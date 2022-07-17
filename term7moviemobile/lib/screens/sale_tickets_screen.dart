import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/sale_tickets_controller.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/utils/theme.dart';

class SaleTicketsScreen extends StatefulWidget {
  const SaleTicketsScreen({Key? key}) : super(key: key);

  @override
  State<SaleTicketsScreen> createState() => _SaleTicketsScreenState();
}

class _SaleTicketsScreenState extends State<SaleTicketsScreen> {
  ScrollController scrollController = ScrollController();
  SaleTicketsController controller = Get.put(SaleTicketsController());

  @override
  void initState() {
    super.initState();
    controller.fetchData();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Obx(() => LoadingOverlay(
            isLoading: controller.isLoading.value,
            color: MyTheme.backgroundColor,
            opacity: 1,
            progressIndicator: const CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
            ),
            child: SafeArea(
              child: Container(
                child: Column(
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(left: 12, top: 36),
                      child: Text(
                        "Flash sale".toUpperCase(),
                        style: TextStyle(
                            color: MyTheme.primaryColor,
                            fontWeight: FontWeight.w700),
                      ),
                    ),
                    RefreshIndicator(
                      child: ListView.builder(
                        padding: EdgeInsets.only(top: 20),
                        scrollDirection: Axis.vertical,
                        shrinkWrap: true,
                        itemBuilder: (context, index) => Container(
                          margin:
                          const EdgeInsets.only(left: 16.0, right: 16),
                          alignment: Alignment.center,
                          child: GestureDetector(
                            onTap: () {
                              List<TicketModel> list = [];
                              list.add(controller.tickets[index]);
                              Get.toNamed("/checkout", arguments: {
                                'showtime':
                                controller.tickets[index].showtime,
                                'tickets': list,
                                'total':
                                controller.tickets[index].sellingPrice
                              });
                            },
                            child: Container(
                              height: 140,
                              child: Stack(
                                children: [
                                  Positioned(
                                    bottom: 0,
                                    left: 0,
                                    right: 0,
                                    child: Container(
                                      height: 140,
                                      decoration: BoxDecoration(
                                        borderRadius:
                                        BorderRadius.circular(8),
                                        color: MyTheme.borderColor,
                                      ),
                                    ),
                                  ),
                                  Positioned(
                                    top: -15,
                                    bottom: -15,
                                    right: 70,
                                    child: Column(
                                      mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                      crossAxisAlignment:
                                      CrossAxisAlignment.center,
                                      children: [
                                        CircleAvatar(
                                          radius: 14,
                                          backgroundColor:
                                          MyTheme.backgroundColor,
                                        ),
                                        CircleAvatar(
                                          radius: 14,
                                          backgroundColor:
                                          MyTheme.backgroundColor,
                                        )
                                      ],
                                    ),
                                  ),
                                  Positioned(
                                    top: 3,
                                    left: 5,
                                    right: 0,
                                    child: Row(
                                      children: [
                                        ClipRRect(
                                          borderRadius:
                                          BorderRadius.circular(6),
                                          child: Image.network(
                                            controller
                                                .tickets[index]
                                                .showtime!
                                                .movie!
                                                .posterImgUrl!
                                                .length ==
                                                0
                                                ? 'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png'
                                                : controller
                                                .tickets[index]
                                                .showtime!
                                                .movie!
                                                .posterImgUrl!,
                                            height: 130,
                                            width: 90,
                                            fit: BoxFit.cover,
                                          ),
                                        ),
                                        Padding(
                                          padding: const EdgeInsets.all(8.0),
                                          child: Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: [
                                              Text("Name", style: TextStyle(color: Colors.black38, fontSize: 10),),
                                              Container(
                                                width: 130,
                                                child: Text(
                                                  controller
                                                      .tickets[index]
                                                      .showtime!
                                                      .movie!
                                                      .title ??
                                                      '',
                                                  maxLines: 2,
                                                  overflow: TextOverflow.ellipsis,
                                                  softWrap: true,
                                                  style: TextStyle(
                                                      fontSize: 16,
                                                      color: Colors.black
                                                          .withOpacity(0.8),
                                                      fontWeight:
                                                      FontWeight.w600),
                                                ),
                                              ),
                                              SizedBox(height: 4),
                                              Text("Date", style: TextStyle(color: Colors.black38, fontSize: 10),),
                                              Text(
                                                DateFormat.Hm().format(
                                                    DateTime.parse(controller
                                                        .tickets[
                                                    index]
                                                        .showtime!
                                                        .startTime!)
                                                        .add(Duration(
                                                        hours:
                                                        7))) +
                                                    ' - ' +
                                                    DateFormat.yMMMd()
                                                        .format(DateTime
                                                        .parse(controller
                                                        .tickets[
                                                    index]
                                                        .showtime!
                                                        .startTime!))
                                                        .toString(),
                                                style: TextStyle(
                                                    fontSize: 14,
                                                    fontWeight:
                                                    FontWeight.w500),
                                              ),
                                              const SizedBox(
                                                height: 4,
                                              ),
                                              Text("Theater", style: TextStyle(color: Colors.black38, fontSize: 10),),
                                              Text(
                                                controller.tickets[index].showtime?.theaterName ?? '',
                                                style: TextStyle(
                                                    fontSize: 14,
                                                    fontWeight:
                                                    FontWeight.w500),
                                              ),

                                            ],
                                          ),
                                        )
                                      ],
                                    ),
                                  ),
                                  Positioned(
                                    top: 10,
                                    right: 0,
                                    child: Row(
                                      children: [
                                        Padding(
                                          padding: const EdgeInsets.all(4.0),
                                          child: Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: [
                                              Text("Seat", style: TextStyle(color: Colors.black38, fontSize: 10),),
                                              Text(
                                                  controller.tickets[index].seat!.name!,
                                              ),
                                              const SizedBox(
                                                height: 40,
                                              ),
                                              Text("Price", style: TextStyle(color: Colors.black38, fontSize: 10),),
                                              Text(
                                                  controller.tickets[index]
                                                                .sellingPrice!
                                                                .toStringAsFixed(
                                                                0) +
                                                                ' VND',
                                                style: TextStyle(
                                                    fontSize: 14,
                                                    fontWeight:
                                                    FontWeight.w500),
                                              ),
                                            ],
                                          ),
                                        )
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ),
                        itemCount: controller.tickets.length,
                      ),
                      color: MyTheme.primaryColor,
                      onRefresh: () async {
                        controller.fetchData();
                      },
                    )
                  ],
                ),
              ),
            ),
          )),
    );
  }
}
