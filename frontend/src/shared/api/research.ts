import { useAuth } from '@clerk/clerk-react';
import { useMemo } from 'react';
import { registerGetToken, resolveAuthToken } from './authTokenBridge';
import { ApiClient } from './client';

export type ResearchStatus = 'draft' | 'pending' | 'analyzing' | 'analyzed' | 'failed';
export type RiskLevel = 'low' | 'medium' | 'high';
export type InputMethod = 'form' | 'text';

export interface ResearchCarSummaryDto {
  make: string | null;
  model: string | null;
  year: number | null;
  mileageKm: number | null;
  price: number | null;
}

export interface CarDataDto extends ResearchCarSummaryDto {
  currency: string | null;
  vin: string | null;
  fuelType: string | null;
  transmission: string | null;
  engineDisplacement: string | null;
  color: string | null;
  condition: string | null;
}

export interface ResearchListItemDto {
  id: string;
  name: string | null;
  status: ResearchStatus | null;
  riskLevel: RiskLevel | null;
  car: ResearchCarSummaryDto | null;
  creditsSpent: number;
  updatedAt: string;
}

export interface ResearchDetailDto {
  id: string;
  name: string | null;
  status: ResearchStatus | null;
  riskLevel: RiskLevel | null;
  inputMethod: InputMethod;
  car: CarDataDto | null;
  description: string | null;
  descriptionSource: string | null;
  creditsSpent: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCarDataDto {
  make?: string | null;
  model?: string | null;
  year?: number | null;
  mileageKm?: number | null;
  price?: number | null;
  currency?: string | null;
  vin?: string | null;
  fuelType?: string | null;
  transmission?: string | null;
  engineDisplacement?: string | null;
  color?: string | null;
  condition?: string | null;
}

export interface CreateResearchDto {
  inputMethod: InputMethod;
  car?: CreateCarDataDto | null;
}

export interface PaginatedResult<T> {
  items: T[];
  total: number;
}

export interface IResearchApi {
  list(page?: number, pageSize?: number): Promise<PaginatedResult<ResearchListItemDto>>;
  create(dto: CreateResearchDto): Promise<ResearchDetailDto>;
  get(id: string): Promise<ResearchDetailDto>;
  rename(id: string, newName: string): Promise<ResearchDetailDto>;
  delete(id: string): Promise<void>;
}

async function readErrorMessage(res: Response): Promise<string> {
  const body = await res.text();
  return body || `API error: ${res.statusText}`;
}

const listInflight = new Map<string, Promise<PaginatedResult<ResearchListItemDto>>>();

function dedupeListRequest(
  key: string,
  execute: () => Promise<PaginatedResult<ResearchListItemDto>>,
): Promise<PaginatedResult<ResearchListItemDto>> {
  const existing = listInflight.get(key);
  if (existing) {
    return existing;
  }

  const promise = execute().finally(() => {
    listInflight.delete(key);
  });
  listInflight.set(key, promise);
  return promise;
}

export class ResearchApiClient extends ApiClient implements IResearchApi {
  async list(page = 1, pageSize = 20): Promise<PaginatedResult<ResearchListItemDto>> {
    const cacheKey = `${page}:${pageSize}`;
    return dedupeListRequest(cacheKey, async () => {
      const res = await this.request(`research?page=${page}&pageSize=${pageSize}`);
      if (!res.ok) {
        throw new Error(await readErrorMessage(res));
      }
      return res.json() as Promise<PaginatedResult<ResearchListItemDto>>;
    });
  }

  async create(dto: CreateResearchDto): Promise<ResearchDetailDto> {
    const res = await this.request('research', {
      method: 'post',
      json: dto,
    });
    if (!res.ok) {
      throw new Error(await readErrorMessage(res));
    }
    return res.json() as Promise<ResearchDetailDto>;
  }

  async get(id: string): Promise<ResearchDetailDto> {
    const res = await this.request(`research/${id}`);
    if (!res.ok) {
      throw new Error(await readErrorMessage(res));
    }
    return res.json() as Promise<ResearchDetailDto>;
  }

  async rename(id: string, newName: string): Promise<ResearchDetailDto> {
    const res = await this.request(`research/${id}/name`, {
      method: 'patch',
      json: { newName },
    });
    if (!res.ok) {
      throw new Error(await readErrorMessage(res));
    }
    return res.json() as Promise<ResearchDetailDto>;
  }

  async delete(id: string): Promise<void> {
    const res = await this.request(`research/${id}`, { method: 'delete' });
    if (!res.ok) {
      throw new Error(await readErrorMessage(res));
    }
  }
}

export function useResearchApi(): IResearchApi {
  const { getToken } = useAuth();
  registerGetToken(getToken);

  return useMemo(() => new ResearchApiClient(resolveAuthToken), []);
}
