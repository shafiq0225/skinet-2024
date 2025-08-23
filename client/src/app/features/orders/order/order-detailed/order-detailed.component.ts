import { Component, inject } from '@angular/core';
import { OrderService } from '../../../../core/services/order.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AccountService } from '../../../../core/services/account.service';
import { Order } from '../../../../shared/models/order';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { AddressPipe } from "../../../../shared/pipes/address-pipe";
import { PaymentCardPipe } from "../../../../shared/pipes/payment-card-pipe";

@Component({
  selector: 'app-order-detailed',
  imports: [MatButton, MatCardModule, DatePipe, CurrencyPipe, AddressPipe, PaymentCardPipe, RouterLink],
  templateUrl: './order-detailed.component.html',
  styleUrl: './order-detailed.component.scss'
})
export class OrderDetailedComponent {
  private orderService = inject(OrderService);
  private activatedRoute = inject(ActivatedRoute);
  // private accountService = inject(AccountService);
  // private adminService = inject(AdminService);
  // private router = inject(Router);
  order?: Order;
  // buttonText = this.accountService.isAdmin() ? 'Return to admin' : 'Return to orders'

  ngOnInit(): void {
    this.loadOrder();
  }

  // onReturnClick() {
  //   this.accountService.isAdmin()
  //     ? this.router.navigateByUrl('/admin')
  //     : this.router.navigateByUrl('/orders')
  // }

  loadOrder() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) return;
    this.orderService.getOrderDetailed(+id).subscribe({
      next: order => this.order = order
    });
    // const loadOrderData = this.accountService.isAdmin()
    //   ? this.adminService.getOrder(+id)
    //   : this.orderService.getOrderDetailed(+id);

    // loadOrderData.subscribe({
    //   next: order => this.order = order
    // })
  }
}
