import { registerClientUser } from "@/api/clientUser";
import type { ClientUserRegisterRequest } from "@/api/model";

export class RegistryApiService {
  async register(requestData: ClientUserRegisterRequest): Promise<number> {
    const response = await registerClientUser(requestData);
    return response;
  }
}
